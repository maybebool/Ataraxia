using MTA;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace Exercises.ExerciseOne {
    public class GazeRaycastIndicator : MTABase {
        
        [SerializeField] private XRRayInteractor ray;
        [SerializeField] private GameObject objectToSpawn;
        [SerializeField] private float headTremorDecayRate = 5f;
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

        protected override float LastUpdateTime { get; set; }

        protected override void Start() {
            LastUpdateTime = Time.time;
            _spawnedObject = Instantiate(objectToSpawn, _currentRayEndPoint, Quaternion.identity);
            UpdateRayEndPointAndPosition();
        }

        protected override void Update() {
            if (!IsCollectingData) {
                TremorIntensity -= headTremorDecayRate * Time.deltaTime;
                TremorIntensity = Mathf.Clamp(TremorIntensity, 0f, 10f);
            }
            UpdateRayEndPointAndPosition();
        }

        private void UpdateRayEndPointAndPosition() {
            _currentRayEndPoint = ray.rayEndPoint;
            _spawnedObject.transform.position = _currentRayEndPoint;
        }
    }
}