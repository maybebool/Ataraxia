using Managers;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace MTA {
    public class LeftLegTremorDetection : MTABase {
        
        [SerializeField] private XRRayInteractor ray;

        protected override XRRayInteractor RaycastPoint {
            get => ray;
            set => ray = value;
        }
        
        protected override Vector3 CurrentPos {
            get => scO.leftLegCurrentPos;
            set => scO.leftLegCurrentPos = value;
        }

        protected override float Degree {
            get => scO.leftLegDegree;
            set => scO.leftLegDegree = value;
        }

        protected override float TremorIntensity {
            get => scO.tremorIntensityLeftLeg;
            set => scO.tremorIntensityLeftLeg = value;
        }

        protected override bool IsCollectingData {
            get => scO.isLeftLegCollectingData;
            set => scO.isLeftLegCollectingData = value;
        }

        protected override float IntensityMultiplier {
            get => scO.leftLegIntensityMultiplier; 
            set => scO.leftLegIntensityMultiplier = value;
        }
        
        protected override float OscillationThreshold {
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