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

        protected override void OnEnable() {
            base.OnEnable();
            if (MtsEventManager.Instance != null) {
                MtsEventManager.Instance.OnLeftHandBtnPressed += OnLeftHandBtnPressed;
                MtsEventManager.Instance.OnLeftHandBtnReleased += OnLeftHandButtonReleased;
            }
        }

        protected override void OnDisable() {
            if (MtsEventManager.Instance != null) {
                MtsEventManager.Instance.OnLeftHandBtnPressed -= OnLeftHandBtnPressed;
                MtsEventManager.Instance.OnLeftHandBtnReleased -= OnLeftHandButtonReleased;
            }

            base.OnDisable();
        }

        private void OnLeftHandBtnPressed() {
            Debug.Log("Left Hand Button Pressed");
            StartDataCollection();
        }

        private void OnLeftHandButtonReleased() {
            Debug.Log("Left Hand Button Released");
            StopDataCollection();
        }
    }
}