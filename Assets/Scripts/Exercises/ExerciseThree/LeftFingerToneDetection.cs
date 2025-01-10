using Managers;
using UnityEngine.InputSystem;

namespace Exercises.ExerciseThree {
    public class LeftFingerToneDetection : MTAToneBase {
        protected override float NewYAxisScaleValue { 
            get => scO.leftPlayerObjectHeight; 
            set => scO.leftPlayerObjectHeight = value; }

        protected override float BtnPressureValue { get; set; } = 0;

        protected override bool IsCollectingData {
            get => scO.isLeftFingerToneCollectingData;
            set => scO.isLeftFingerToneCollectingData = value ;
        }

        protected override void OnEnable() {
            if (MtsEventManager.Instance != null) {
                MtsEventManager.Instance.OnLeftHandFingerToneBtnPressed += OnLeftHandFingerToneBtnPressed;
                MtsEventManager.Instance.OnLeftHandFingerToneBtnReleased += OnLeftHandFingerToneBtnReleased;
            }
        }

        protected override void OnDisable() {
            if (MtsEventManager.Instance != null) {
                MtsEventManager.Instance.OnLeftHandFingerToneBtnPressed -= OnLeftHandFingerToneBtnPressed;
                MtsEventManager.Instance.OnLeftHandFingerToneBtnReleased -= OnLeftHandFingerToneBtnReleased;
            }
        }
        
        private void OnLeftHandFingerToneBtnPressed(InputAction.CallbackContext context) {
            BtnPressureValue = context.ReadValue<float>();
            StartToneCollection();
        }

        private void OnLeftHandFingerToneBtnReleased(InputAction.CallbackContext context) {
            BtnPressureValue = 0f;
            StopToneCollection();
        }
    }
}