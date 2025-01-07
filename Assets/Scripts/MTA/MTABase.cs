using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace MTA {
    public abstract class MTABase : MonoBehaviour {
        [Header("References")] public DataContainer scO;
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

        [Tooltip("Extra time window to validate repeated threshold passes.")] [SerializeField]
        private float timeThreshold = 0.2f;

        [Tooltip("If the position change is below this threshold, we skip tremor calculations.")]
        public float positionChangeThreshold = 0.01f;

        // This will store whether we've recently passed the threshold
        private bool hasSurpassedThresholdFirstTime = false;

        // The exact moment we first passed the threshold
        private float firstSurpassTimestamp = 0f;

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
                    var distance = Vector3.Distance(CurrentPos, previousPosition);
                    if (distance > positionChangeThreshold) {
                        // Horizontal & vertical displacements
                        var deltaX = CurrentPos.x - previousPosition.x;
                        var deltaY = CurrentPos.y - previousPosition.y;

                        // 0° at positive Y axis, so we do Atan2(x, y) instead of Atan2(y, x)
                        var angleRadians = Mathf.Atan2(deltaX, deltaY);

                        // Ensure angle is in [0, 2π)
                        if (angleRadians < 0f) angleRadians += PIDoubled;

                        // Convert to degrees in [0, 360)
                        var newDegree = angleRadians * Mathf.Rad2Deg;
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

            // Get the shortest signed difference in [-180, 180]
            var deltaDegree = Mathf.DeltaAngle(previousDegree, bodyPartDegree);
            var speed = Mathf.Abs(deltaDegree / deltaTime);

            if (speed > speedThreshold) {
                detector.GetComponent<Renderer>().material.color = Color.blue;
            }

            oscillationDelta += deltaDegree;

            if (Mathf.Abs(oscillationDelta) > OscillationThreshold) {
                if (!hasSurpassedThresholdFirstTime) {
                    hasSurpassedThresholdFirstTime = true;
                    firstSurpassTimestamp = currentTime;
                    oscillationDelta = 0f;
                }else {
                    if ((currentTime - firstSurpassTimestamp) <= timeThreshold) {
                        detector.GetComponent<Renderer>().material.color = Color.red;
                        oscillationDelta = 0f;
                        IncrementTremorIntensityValue();
                    }else {
                        hasSurpassedThresholdFirstTime = false;
                        oscillationDelta = 0f;
                    }
                }
            }

            if (hasSurpassedThresholdFirstTime &&
                (currentTime - firstSurpassTimestamp > timeThreshold)) {
                hasSurpassedThresholdFirstTime = false;
            }

            previousDegree = bodyPartDegree;
            previousDelta = deltaDegree;
            lastUpdateTime = currentTime;
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