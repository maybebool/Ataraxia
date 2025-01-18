using Managers;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace MTA {
    public class LeftHandTremorDetection : MTATremorBase {
        [SerializeField] private XRRayInteractor ray;

        protected override XRRayInteractor raycastPoint {
            get => ray;
            set => ray = value;
        }

        protected override Vector3 currentPos {
            get => scO.leftHandCurrentPos;
            set => scO.leftHandCurrentPos = value;
        }

        protected override float degree {
            get => scO.leftHandDegree;
            set => scO.leftHandDegree = value;
        }

        protected override float tremorIntensity {
            get => scO.tremorIntensityLeftHand;
            set => scO.tremorIntensityLeftHand = value;
        }
        
        protected override bool isCollectingData {
            get => scO.isLeftHandCollectingData;
            set => scO.isLeftHandCollectingData = value;
        }
        
        protected override float intensityMultiplier {
            get => scO.leftHandIntensityMultiplier; 
            set => scO.leftHandIntensityMultiplier = value;
        }
        
        protected override int oscillationThreshold {
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