using Managers;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace MTA {
    public class RightHandTremorDetection : MTATremorBase {
        [SerializeField] private XRRayInteractor ray;

        protected override XRRayInteractor raycastPoint {
            get => ray;
            set => ray = value;
        }
        
        protected override Vector3 currentPos {
            get => scO.rightHandCurrentPos;
            set => scO.rightHandCurrentPos = value;
        }

        protected override float degree {
            get => scO.rightHandDegree;
            set => scO.rightHandDegree = value;
        }

        protected override float tremorIntensity {
            get => scO.tremorIntensityRightHand;
            set => scO.tremorIntensityRightHand = value;
        }

        protected override bool isCollectingData {
            get => scO.isRightHandCollectingData;
            set => scO.isRightHandCollectingData = value;
        }

        protected override float intensityMultiplier {
            get => scO.rightHandIntensityMultiplier; 
            set => scO.rightHandIntensityMultiplier = value;
        }
        
        protected override int oscillationThreshold {
            get => scO.rightHandOscillationThreshold;
            set => scO.rightHandOscillationThreshold = value;
        }

        protected override void OnEnable() {
            base.OnEnable();
            if (MtsEventManager.Instance != null) {
                MtsEventManager.Instance.OnRightHandBtnPressed += OnRightHandBtnPressed;
                MtsEventManager.Instance.OnRightHandBtnReleased += OnRightHandButtonReleased;
            }
        }

        protected override void OnDisable() {
            if (MtsEventManager.Instance != null) {
                MtsEventManager.Instance.OnRightHandBtnPressed -= OnRightHandBtnPressed;
                MtsEventManager.Instance.OnRightHandBtnReleased -= OnRightHandButtonReleased;
            }
            base.OnDisable();
        }

        private void OnRightHandBtnPressed() {
            StartDataCollection();
        }

        private void OnRightHandButtonReleased() {
            StopDataCollection();
        }
    }
}