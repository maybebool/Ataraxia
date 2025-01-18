using Managers;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace MTA {
    public class LeftLegTremorDetection : MTATremorBase {
        
        [SerializeField] private XRRayInteractor ray;

        protected override XRRayInteractor raycastPoint {
            get => ray;
            set => ray = value;
        }
        
        protected override Vector3 currentPos {
            get => scO.leftLegCurrentPos;
            set => scO.leftLegCurrentPos = value;
        }

        protected override float degree {
            get => scO.leftLegDegree;
            set => scO.leftLegDegree = value;
        }

        protected override float tremorIntensity {
            get => scO.tremorIntensityLeftLeg;
            set => scO.tremorIntensityLeftLeg = value;
        }

        protected override bool isCollectingData {
            get => scO.isLeftLegCollectingData;
            set => scO.isLeftLegCollectingData = value;
        }

        protected override float intensityMultiplier {
            get => scO.leftLegIntensityMultiplier; 
            set => scO.leftLegIntensityMultiplier = value;
        }
        
        protected override int oscillationThreshold {
            get => scO.leftLegOscillationThreshold;
            set => scO.leftLegOscillationThreshold = value;
        }

        protected override void OnEnable() {
            base.OnEnable();
            if (MtsEventManager.Instance != null) {
                MtsEventManager.Instance.OnLeftLegBtnPressed += OnLeftLegBtnPressed;
                MtsEventManager.Instance.OnLeftLegBtnReleased += OnLeftLegButtonReleased;
            }
        }

        protected override void OnDisable() {
            if (MtsEventManager.Instance != null) {
                MtsEventManager.Instance.OnLeftLegBtnPressed -= OnLeftLegBtnPressed;
                MtsEventManager.Instance.OnLeftLegBtnReleased -= OnLeftLegButtonReleased;
            }
            base.OnDisable();
        }

        private void OnLeftLegBtnPressed() {
            StartDataCollection();
        }

        private void OnLeftLegButtonReleased() {
            StopDataCollection();
        }
    }
}