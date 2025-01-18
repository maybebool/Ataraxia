using Managers;
using MTA;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace Exercises.ExerciseOne {
    public class GazeTremorDetection : MTATremorBase {
        
        [SerializeField] private XRRayInteractor ray;
        [SerializeField] private GameObject objectToSpawn;
        private Vector3 _currentRayEndPoint;

        protected override XRRayInteractor raycastPoint {
            get => ray;
            set => ray = value;
        }

        protected override Vector3 currentPos {
            get => scO.headCurrentPos;
            set => scO.headCurrentPos = value;
        }

        protected override float degree {
            get => scO.headDegree;
            set => scO.headDegree = value;
        }

        protected override float tremorIntensity {
            get => scO.tremorIntensityHead;
            set => scO.tremorIntensityHead = value;
        }
        
        protected override bool isCollectingData {
            get => scO.isHeadCollectingData;
            set => scO.isHeadCollectingData = value;
        }
        
        protected override float intensityMultiplier {
            get => scO.headIntensityMultiplier; 
            set => scO.headIntensityMultiplier = value;
        }
        
        protected override int oscillationThreshold {
            get => scO.headOscillationThreshold;
            set => scO.headOscillationThreshold = value;
        }

        protected override void Start() {
            lastUpdateTime = Time.time;
        }

        protected override void OnEnable() {
            base.OnEnable();
            if (MtsEventManager.Instance != null) {
                MtsEventManager.Instance.OnHeadActionActivated += OnHeadActionActivated;
                MtsEventManager.Instance.OnHeadActionDeactivated += OnHeadActionDeactivated;
            }
            objectToSpawn.transform.position = _currentRayEndPoint;
            objectToSpawn.transform.rotation = Quaternion.identity;
            UpdateRayEndPointAndPosition();
        }

        protected override void OnDisable() {
            base.OnDisable();
            if (MtsEventManager.Instance != null) {
                MtsEventManager.Instance.OnHeadActionActivated -= OnHeadActionActivated;
                MtsEventManager.Instance.OnHeadActionDeactivated -= OnHeadActionDeactivated;
            }
        }

        protected override void Update() {
            if (!isCollectingData) {
                tremorIntensity -= tremorDecayRate * Time.deltaTime;
                tremorIntensity = Mathf.Clamp(tremorIntensity, 0f, 10f);
            }
            UpdateRayEndPointAndPosition();
        }

        private void UpdateRayEndPointAndPosition() {
            _currentRayEndPoint = ray.rayEndPoint;
            if (objectToSpawn) {
                objectToSpawn.transform.position = _currentRayEndPoint;
            }
        }
        
        private void OnHeadActionActivated() {
            StartDataCollection();
        }
        
        private void OnHeadActionDeactivated() {
            StopDataCollection();
        }
    }
}