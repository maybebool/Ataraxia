using Managers;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace MTA {
    public class RightLegTremorDetection : MTATremorBase{
        [SerializeField] private XRRayInteractor ray;

        protected override XRRayInteractor RaycastPoint {
            get => ray;
            set => ray = value;
        }
        
        protected override Vector3 CurrentPos {
            get => scO.rightLegCurrentPos;
            set => scO.rightLegCurrentPos = value;
        }

        protected override float Degree {
            get => scO.rightLegDegree;
            set => scO.rightLegDegree = value;
        }

        protected override float TremorIntensity {
            get => scO.tremorIntensityRightLeg;
            set => scO.tremorIntensityRightLeg = value;
        }

        protected override bool IsCollectingData {
            get => scO.isRightLegCollectingData;
            set => scO.isRightLegCollectingData = value;
        }

        protected override float IntensityMultiplier {
            get => scO.rightLegIntensityMultiplier; 
            set => scO.rightLegIntensityMultiplier = value;
        }
        
        protected override float OscillationThreshold {
            get => scO.rightLegOscillationThreshold;
            set => scO.rightLegOscillationThreshold = value;
        }

        protected override void OnEnable() {
            base.OnEnable();
            if (MtsEventManager.Instance != null) {
                MtsEventManager.Instance.OnRightLegBtnPressed += OnRightLegBtnPressed;
                MtsEventManager.Instance.OnRightLegBtnReleased += OnRightLegButtonReleased;
            }
        }

        protected override void OnDisable() {
            if (MtsEventManager.Instance != null) {
                MtsEventManager.Instance.OnRightLegBtnPressed -= OnRightLegBtnPressed;
                MtsEventManager.Instance.OnRightLegBtnReleased -= OnRightLegButtonReleased;
            }
            base.OnDisable();
        }

        private void OnRightLegBtnPressed() {
            StartDataCollection();
        }

        private void OnRightLegButtonReleased() {
            StopDataCollection();
        }
    }
}