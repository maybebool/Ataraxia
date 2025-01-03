using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace MTA {
    public abstract class MTABase : MonoBehaviour {
        [Header("References")] 
        public DataContainer scO;
        public GameObject detector;
    
        [Header("Parameters")] 
        public Vector4 _outterCircle;
        public float _tangentCircleRadius;
        public float speedThreshold = 50f;
        public float oscillationThreshold = 140f;
    
        private float previousDegree;
        private float previousDelta;
        private float oscillationDelta;
        private float lastUpdateTime;
        private float tremorIntensity = 0f;
        private float tremorDecayRate = 5f;
        private List<float> tremorEventTimes = new();
        private float timeWindow = 3f;
        private float multiplierThreshold = 3f;
        private Vector3 previousPosition;
        private bool hasPreviousPosition = false;
    
        private Vector3 outterCirclePosition;
        private Vector3 outterCircleScale;
        private Vector3 tangentCircleScale;
        private Coroutine dataCollectionCoroutine;

        private const float PiHalf = Mathf.PI / 2;
        private const float PIDoubled = 2 * Mathf.PI;

    
        protected abstract XRRayInteractor RaycastPoint { get; set; } // Where we do our raycast
        protected abstract Vector3 CurrentPos { get; set; }
        protected abstract float Degree { get; set; }
        protected abstract float TremorIntensity { get; set; }
        protected abstract bool IsCollectingData { get; set; }

        private void Start() {
            lastUpdateTime = Time.time;
            outterCircleScale = new Vector3(_outterCircle.w, _outterCircle.w, _outterCircle.w) * 2f;
            tangentCircleScale = new Vector3(_tangentCircleRadius, _tangentCircleRadius, _tangentCircleRadius) * 2f;
        }

        protected virtual void OnEnable() {
        }

        protected virtual void OnDisable() {
        }

        private void Update() {
            if (!IsCollectingData) {
                tremorIntensity -= tremorDecayRate * Time.deltaTime;
                tremorIntensity = Mathf.Clamp(tremorIntensity, 0f, 10f);
            }
        }

        protected void StartDataCollection() {
            IsCollectingData = true;
            ResetTremorDetectionVariables();
            dataCollectionCoroutine = StartCoroutine(DataCollectionRoutine());
        }

        protected void StopDataCollection() {
            IsCollectingData = false;
            if (dataCollectionCoroutine != null) {
                StopCoroutine(dataCollectionCoroutine);
                dataCollectionCoroutine = null;
            }

            hasPreviousPosition = false;
        }

        private IEnumerator DataCollectionRoutine() {
            while (IsCollectingData) {
                // 1) Raycast
                RaycastPoint.TryGetCurrent3DRaycastHit(out var hit);

                // 2) Assign CurrentPos from the hit
                CurrentPos = hit.point;

                // 3) If we have a previous position, do calculations
                if (hasPreviousPosition) {
                    float deltaX = previousPosition.x - CurrentPos.x;
                    float deltaY = previousPosition.y - CurrentPos.y;
                    float hypotenuse = CalculateHypotenuse(previousPosition, CurrentPos);

                    float quadrantRadiant = CalculateQuadrantLogicForRadiant(deltaX, deltaY, hypotenuse);
                    float newDegree = quadrantRadiant * Mathf.Rad2Deg;

                    // Save to the correct property
                    Degree = newDegree;

                    // Calculate tremor
                    CalculateTremor();

                    // Decay
                    tremorIntensity -= tremorDecayRate * Time.deltaTime;
                    tremorIntensity = Mathf.Clamp(tremorIntensity, 0f, 10f);

                    // Store final tremor in the correct property
                    TremorIntensity = tremorIntensity;
                }
                else {
                    // First time only
                    previousDegree = Degree;
                    lastUpdateTime = Time.time;
                    previousDelta = 0f;
                }

                // 4) Update the local 'previousPosition'
                previousPosition = CurrentPos;
                hasPreviousPosition = true;

                yield return null; // each frame
            }
        }

        private void ResetTremorDetectionVariables() {
            previousDegree = 0f;
            previousDelta = 0f;
            oscillationDelta = 0f;
            tremorIntensity = 0f;
            tremorEventTimes.Clear();
        }

        private void CalculateTremor() {
            float currentTime = Time.time;
            float deltaTime = currentTime - lastUpdateTime;

            // Read the current degree from the abstract property
            float bodyPartDegree = Degree;

            float deltaDegree = bodyPartDegree - previousDegree;
            if (deltaDegree > 180f) deltaDegree -= 360f;
            if (deltaDegree < -180f) deltaDegree += 360f;

            float speed = Mathf.Abs(deltaDegree / deltaTime);

            // Color change for speed threshold
            if (speed > speedThreshold) {
                detector.GetComponent<Renderer>().material.color = Color.blue;
            }

            // Check oscillation
            oscillationDelta = previousDelta + deltaDegree;
            if (Mathf.Abs(oscillationDelta) > oscillationThreshold) {
                detector.GetComponent<Renderer>().material.color = Color.red;
                oscillationDelta = 0f;
                IncrementTremorIntensityValue();
            }

            // Update previous
            previousDegree = bodyPartDegree;
            previousDelta = deltaDegree;
            lastUpdateTime = currentTime;
        }

        private Vector3 GetRadiantPointReflection(float radiant, float scale) {
            var sin = scale * (float)Math.Sin(radiant);
            var cos = scale * (float)Math.Cos(radiant);
            return new Vector3(sin, -cos, 0);
        }

        private float CalculateHypotenuse(Vector3 pointA, Vector3 pointB) {
            return Vector3.Distance(pointA, pointB);
        }

        private float CalculateQuadrantLogicForRadiant(float deltaX, float deltaY, float hypotenuse) {
            if (Mathf.Approximately(hypotenuse, 0f))
                return 0f;

            float thetaOfCos = Mathf.Acos(deltaX / hypotenuse);
            float quadrantBasedRadiant = 0f;

            // Quadrant logic example
            quadrantBasedRadiant = deltaX switch {
                // top left Quadrant > < | 0 - 90
                > 0 when deltaY < 0 => PiHalf - thetaOfCos,
                // bottom left Quadrant > > | 90 - 180
                > 0 when deltaY > 0 => PiHalf + thetaOfCos,
                // bottom right Quadrant < > | 180 - 270
                < 0 when deltaY > 0 => PIDoubled + PiHalf - thetaOfCos,
                // top right Quadrant < <  | 270 - 360
                < 0 when deltaY < 0 => PIDoubled + PiHalf - thetaOfCos,
                _ => quadrantBasedRadiant
            };

            return quadrantBasedRadiant;
        }

        private void IncrementTremorIntensityValue() {
            float incrementAmount = 0.3f;
            tremorEventTimes.Add(Time.time);
            tremorEventTimes.RemoveAll(t => t < Time.time - timeWindow);

            int eventCount = tremorEventTimes.Count;
            if (eventCount >= multiplierThreshold) {
                float multiplier = 1f + (eventCount - multiplierThreshold) * 0.01f;
                incrementAmount *= multiplier;
            }

            tremorIntensity += incrementAmount;
            tremorIntensity = Mathf.Clamp(tremorIntensity, 0f, 10f);
        }
    }
}

