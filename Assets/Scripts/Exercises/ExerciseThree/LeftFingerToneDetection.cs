using Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Exercises.ExerciseThree {
    public class LeftFingerToneDetection : MTAToneBase {
        protected override float NewYAxisScaleValue { 
            get => scO.leftPlayerObjectHeight; 
            set => scO.leftPlayerObjectHeight = value; }

        protected override float BtnPressureValue { get; set; } = 0;

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
            Debug.Log($"Trigger pressed with a value of: {BtnPressureValue}");
        }

        private void OnLeftHandFingerToneBtnReleased(InputAction.CallbackContext context) {
            BtnPressureValue = 0f;
            Debug.Log("Left hand finger tonus btn released");
        }
    }
}