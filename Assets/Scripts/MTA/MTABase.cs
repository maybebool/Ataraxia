using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace MTA {
    public abstract class MTABase : MonoBehaviour {
        [Header("References")] 
        public DataContainer scO;
        public GameObject detector;
    
        [Header("Parameters")] 
        // public Vector4 _outterCircle;
        // public float _tangentCircleRadius;
        public float speedThreshold = 50f;
        public float tremorDecayRate = 5f;
        [HideInInspector] public float lastUpdateTime;
    
        private float previousDegree;
        private float previousDelta;
        private float oscillationDelta;
        private float tremorIntensity = 0f;
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
        protected abstract float IntensityMultiplier { get; set; }
        protected abstract float OscillationThreshold { get; set; }
        
        protected virtual void Start() {
            lastUpdateTime = Time.time;
            // outterCircleScale = new Vector3(_outterCircle.w, _outterCircle.w, _outterCircle.w) * 2f;
            // tangentCircleScale = new Vector3(_tangentCircleRadius, _tangentCircleRadius, _tangentCircleRadius) * 2f;
        }

        protected virtual void OnEnable() {
        }

        protected virtual void OnDisable() {
        }

        protected virtual void Update() {
            if (!IsCollectingData) {
                TremorIntensity -= tremorDecayRate * Time.deltaTime;
                TremorIntensity = Mathf.Clamp(TremorIntensity, 0f, 10f);
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
                RaycastPoint.TryGetCurrent3DRaycastHit(out var hit);
                CurrentPos = hit.point;
                
                if (hasPreviousPosition) {
                    var deltaX = previousPosition.x - CurrentPos.x;
                    var deltaY = previousPosition.y - CurrentPos.y;
                    var hypotenuse = CalculateHypotenuse(previousPosition, CurrentPos);

                    var quadrantRadiant = CalculateQuadrantLogicForRadiant(deltaX, deltaY, hypotenuse);
                    var newDegree = quadrantRadiant * Mathf.Rad2Deg;
                    Degree = newDegree;
                    
                    CalculateTremor();

                    // Decay
                    TremorIntensity -= tremorDecayRate * Time.deltaTime;
                    TremorIntensity = Mathf.Clamp(TremorIntensity, 0f, 10f);
                    
                }
                else {
                    previousDegree = Degree;
                    lastUpdateTime = Time.time;
                    previousDelta = 0f;
                }
                
                previousPosition = CurrentPos;
                hasPreviousPosition = true;

                yield return null;
            }
        }

        private void ResetTremorDetectionVariables() {
            previousDegree = 0f;
            previousDelta = 0f;
            oscillationDelta = 0f;
            TremorIntensity = 0f;
            tremorEventTimes.Clear();
        }

        private void CalculateTremor() {
            var currentTime = Time.time;
            var deltaTime = currentTime - lastUpdateTime;
            var bodyPartDegree = Degree;

            var deltaDegree = bodyPartDegree - previousDegree;
            if (deltaDegree > 180f) deltaDegree -= 360f;
            if (deltaDegree < -180f) deltaDegree += 360f;

            var speed = Mathf.Abs(deltaDegree / deltaTime);
            
            if (speed > speedThreshold) {
                detector.GetComponent<Renderer>().material.color = Color.blue;
            }
            
            oscillationDelta = previousDelta + deltaDegree;
            if (Mathf.Abs(oscillationDelta) > OscillationThreshold) {
                detector.GetComponent<Renderer>().material.color = Color.red;
                oscillationDelta = 0f;
                IncrementTremorIntensityValue();
            }
            
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

            var thetaOfCos = Mathf.Acos(deltaX / hypotenuse);
            var quadrantBasedRadiant = 0f;

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
            var incrementAmount = 0.3f;
            tremorEventTimes.Add(Time.time);
            tremorEventTimes.RemoveAll(t => t < Time.time - timeWindow);

            var eventCount = tremorEventTimes.Count;
            if (eventCount >= multiplierThreshold) {
                var multiplier = 1f + (eventCount - multiplierThreshold) * IntensityMultiplier;
                incrementAmount *= multiplier;
            }

            TremorIntensity += incrementAmount;
            TremorIntensity = Mathf.Clamp(TremorIntensity, 0f, 10f);
        }
    }
}

