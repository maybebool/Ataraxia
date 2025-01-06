using Managers;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace MTA {
    public class LeftHandTremorDetection : MTABase {
        [SerializeField] private XRRayInteractor ray;

        protected override XRRayInteractor RaycastPoint {
            get => ray;
            set => ray = value;
        }

        protected override Vector3 CurrentPos {
            get => scO.leftHandCurrentPos;
            set => scO.leftHandCurrentPos = value;
        }

        protected override float Degree {
            get => scO.leftHandDegree;
            set => scO.leftHandDegree = value;
        }

        protected override float TremorIntensity {
            get => scO.tremorIntensityLeftHand;
            set => scO.tremorIntensityLeftHand = value;
        }
        
        protected override bool IsCollectingData {
            get => scO.isLeftHandCollectingData;
            set => scO.isLeftHandCollectingData = value;
        }
        
        protected override float IntensityMultiplier {
            get => scO.leftHandIntensityMultiplier; 
            set => scO.leftHandIntensityMultiplier = value;
        }
        
        protected override float OscillationThreshold {
            get => scO.leftHandOscillationThreshold;
            set => scO.leftHandOscillationThreshold = value;
        }
        
        protected override void OnEnable() {
            base.OnEnable();
            if (MtsEventManager.Instance != null) {
                MtsEventManager.Instance.OnLeftHandBtnPressed += OnLeftHandBtnPressed;
                MtsEventManager.Instance.OnLeftHandBtnReleased += OnLeftHandButtonReleased;
            }
        }

        protected override void OnDisable() {
            base.OnDisable();
            if (MtsEventManager.Instance != null) {
                MtsEventManager.Instance.OnLeftHandBtnPressed -= OnLeftHandBtnPressed;
                MtsEventManager.Instance.OnLeftHandBtnReleased -= OnLeftHandButtonReleased;
            }

            
        }

        private void OnLeftHandBtnPressed() {
            StartDataCollection();
        }

        private void OnLeftHandButtonReleased() {
            StopDataCollection();
        }
    }
}