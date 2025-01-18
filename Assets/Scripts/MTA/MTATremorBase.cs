using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace MTA {
    public abstract class MTATremorBase : MonoBehaviour {
        [Header("References")] 
        public DataContainer scO;
        public GameObject detector;

        [Header("Parameters")]
        public float speedThreshold = 50f;

        public float tremorDecayRate = 5f;
        [HideInInspector] public float lastUpdateTime;

        private float _previousDegree;
        private float _previousDelta;
        private float _oscillationDelta;
        private float _tremorIntensity = 0f;
        private List<float> _tremorEventTimes = new();
        private float _timeWindow = 3f;
        private float _multiplierThreshold = 3f;
        private Vector3 _previousPosition;
        private bool _hasPreviousPosition = false;

        private Vector3 _outterCirclePosition;
        private Vector3 _outterCircleScale;
        private Vector3 _tangentCircleScale;
        private Coroutine _dataCollectionCoroutine;
        
        [SerializeField] private float timeThreshold = 0.2f;
        
        public float positionChangeThreshold = 0.01f;
        
        private bool _hasSurpassedThresholdFirstTime = false;
        private float _firstSurpassTimestamp = 0f;
        private const float PIDoubled = 2 * Mathf.PI;

        protected abstract XRRayInteractor raycastPoint { get; set; } 
        protected abstract Vector3 currentPos { get; set; }
        protected abstract float degree { get; set; }
        protected abstract float tremorIntensity { get; set; }
        protected abstract bool isCollectingData { get; set; }
        protected abstract float intensityMultiplier { get; set; }
        protected abstract int oscillationThreshold { get; set; }

        protected virtual void Start() {
            lastUpdateTime = Time.time;
        }

        protected virtual void OnEnable() {
        }

        protected virtual void OnDisable() {
        }

        protected virtual void Update() {
            if (!isCollectingData) {
                tremorIntensity -= tremorDecayRate * Time.deltaTime;
                tremorIntensity = Mathf.Clamp(tremorIntensity, 0f, 10f);
            }
        }

        protected void StartDataCollection() {
            isCollectingData = true;
            ResetTremorDetectionVariables();
            _dataCollectionCoroutine = StartCoroutine(DataCollectionRoutine());
        }

        protected void StopDataCollection() {
            isCollectingData = false;
            if (_dataCollectionCoroutine != null) {
                StopCoroutine(_dataCollectionCoroutine);
                _dataCollectionCoroutine = null;
            }

            _hasPreviousPosition = false;
        }

        private IEnumerator DataCollectionRoutine() {
            while (isCollectingData) {
                raycastPoint.TryGetCurrent3DRaycastHit(out var hit);
                currentPos = hit.point;

                if (_hasPreviousPosition) {
                    var distance = Vector3.Distance(currentPos, _previousPosition);
                    if (distance > positionChangeThreshold) {
                        var deltaX = currentPos.x - _previousPosition.x;
                        var deltaY = currentPos.y - _previousPosition.y;

                        // 0° at positive Y axis, so we do Atan2(x, y) instead of Atan2(y, x)
                        var angleRadians = Mathf.Atan2(deltaX, deltaY);

                        // Ensure angle is in [0, 2π)
                        if (angleRadians < 0f) angleRadians += PIDoubled;

                        // Convert to degrees in [0, 360)
                        var newDegree = angleRadians * Mathf.Rad2Deg;
                        degree = newDegree;

                        CalculateTremor();

                        // Decay
                        tremorIntensity -= tremorDecayRate * Time.deltaTime;
                        tremorIntensity = Mathf.Clamp(tremorIntensity, 0f, 10f);
                    }
                    else {
                        _previousDegree = degree;
                        lastUpdateTime = Time.time;
                        _previousDelta = 0f;
                    }
                }
                
                _previousPosition = currentPos;
                _hasPreviousPosition = true;

                yield return null;
            }
        }

        private void ResetTremorDetectionVariables() {
            _previousDegree = 0f;
            _previousDelta = 0f;
            _oscillationDelta = 0f;
            tremorIntensity = 0f;
            _tremorEventTimes.Clear();
        }

        private void CalculateTremor() {
            var currentTime = Time.time;
            var deltaTime = currentTime - lastUpdateTime;
            var bodyPartDegree = degree;

            // Get the shortest signed difference in [-180, 180]
            var deltaDegree = Mathf.DeltaAngle(_previousDegree, bodyPartDegree);
            var speed = Mathf.Abs(deltaDegree / deltaTime);

            if (speed > speedThreshold) {
                detector.GetComponent<Renderer>().material.color = Color.blue;
            }

            _oscillationDelta += deltaDegree;

            if (Mathf.Abs(_oscillationDelta) > oscillationThreshold) {
                if (!_hasSurpassedThresholdFirstTime) {
                    _hasSurpassedThresholdFirstTime = true;
                    _firstSurpassTimestamp = currentTime;
                    _oscillationDelta = 0f;
                }else {
                    if ((currentTime - _firstSurpassTimestamp) <= timeThreshold) {
                        detector.GetComponent<Renderer>().material.color = Color.red;
                        _oscillationDelta = 0f;
                        IncrementTremorIntensityValue();
                    }else {
                        _hasSurpassedThresholdFirstTime = false;
                        _oscillationDelta = 0f;
                    }
                }
            }
            
            if (_hasSurpassedThresholdFirstTime &&
                (currentTime - _firstSurpassTimestamp > timeThreshold)) {
                _hasSurpassedThresholdFirstTime = false;
            }

            _previousDegree = bodyPartDegree;
            _previousDelta = deltaDegree;
            lastUpdateTime = currentTime;
        }

        private void IncrementTremorIntensityValue() {
            var incrementAmount = 0.3f;
            _tremorEventTimes.Add(Time.time);
            _tremorEventTimes.RemoveAll(t => t < Time.time - _timeWindow);

            var eventCount = _tremorEventTimes.Count;
            if (eventCount >= _multiplierThreshold) {
                var multiplier = 1f + (eventCount - _multiplierThreshold) * intensityMultiplier;
                incrementAmount *= multiplier;
            }

            tremorIntensity += incrementAmount;
            tremorIntensity = Mathf.Clamp(tremorIntensity, 0f, 10f);
        }
    }
}