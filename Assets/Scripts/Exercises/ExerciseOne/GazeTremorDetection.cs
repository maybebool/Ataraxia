using Managers;
using MTA;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace Exercises.ExerciseOne {
    public class GazeTremorDetection : MTABase {
        
        [SerializeField] private XRRayInteractor ray;
        [SerializeField] private GameObject objectToSpawn;
        private Vector3 _currentRayEndPoint;
        private GameObject _spawnedObject;

        protected override XRRayInteractor RaycastPoint {
            get => ray;
            set => ray = value;
        }

        protected override Vector3 CurrentPos {
            get => scO.headCurrentPos;
            set => scO.headCurrentPos = value;
        }

        protected override float Degree {
            get => scO.headDegree;
            set => scO.headDegree = value;
        }

        protected override float TremorIntensity {
            get => scO.tremorIntensityHead;
            set => scO.tremorIntensityHead = value;
        }
        
        protected override bool IsCollectingData {
            get => scO.isHeadCollectingData;
            set => scO.isHeadCollectingData = value;
        }
        
        protected override float IntensityMultiplier {
            get => scO.headIntensityMultiplier; 
            set => scO.headIntensityMultiplier = value;
        }
        
        protected override float OscillationThreshold {
            get => scO.headOscillationThreshold;
            set => scO.headOscillationThreshold = value;
            
        }

        protected override void Start() {
            lastUpdateTime = Time.time;
            _spawnedObject = Instantiate(objectToSpawn, _currentRayEndPoint, Quaternion.identity);
            UpdateRayEndPointAndPosition();
        }

        protected override void OnEnable() {
            base.OnEnable();
            if (MtsEventManager.Instance != null) {
                MtsEventManager.Instance.OnHeadActionActivated += OnHeadActionActivated;
                MtsEventManager.Instance.OnHeadActionDeactivated += OnHeadActionDeactivated;
            }
        }

        protected override void OnDisable() {
            base.OnDisable();
            if (MtsEventManager.Instance != null) {
                MtsEventManager.Instance.OnHeadActionActivated -= OnHeadActionActivated;
                MtsEventManager.Instance.OnHeadActionDeactivated -= OnHeadActionDeactivated;
            }
        }

        protected override void Update() {
            if (!IsCollectingData) {
                TremorIntensity -= tremorDecayRate * Time.deltaTime;
                TremorIntensity = Mathf.Clamp(TremorIntensity, 0f, 10f);
            }
            UpdateRayEndPointAndPosition();
        }

        private void UpdateRayEndPointAndPosition() {
            _currentRayEndPoint = ray.rayEndPoint;
            _spawnedObject.transform.position = _currentRayEndPoint;
        }
        
        
        private void OnHeadActionActivated() {
            StartDataCollection();
        }
        
        private void OnHeadActionDeactivated() {
            StopDataCollection();
        }
    }
}