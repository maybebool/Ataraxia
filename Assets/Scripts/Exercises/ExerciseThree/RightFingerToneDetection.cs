using Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Exercises.ExerciseThree {
    public class RightFingerToneDetection : MTAToneBase{
        protected override float NewYAxisScaleValue { 
            get => scO.rightPlayerObjectHeight; 
            set => scO.rightPlayerObjectHeight = value; }
        protected override float BtnPressureValue { get; set; } = 0;

        protected override void OnEnable() {
            if (MtsEventManager.Instance != null) {
                MtsEventManager.Instance.OnRightHandFingerToneBtnPressed += OnRightHandFingerToneBtnPressed;
                MtsEventManager.Instance.OnRightHandFingerToneBtnReleased += OnRightHandFingerToneBtnReleased;
            }
        }

        protected override void OnDisable() {
            if (MtsEventManager.Instance != null) {
                MtsEventManager.Instance.OnRightHandFingerToneBtnPressed -= OnRightHandFingerToneBtnPressed;
                MtsEventManager.Instance.OnRightHandFingerToneBtnReleased -= OnRightHandFingerToneBtnReleased;
            }
        }
        
        private void OnRightHandFingerToneBtnPressed(InputAction.CallbackContext context) {
            scO.isRightFingerToneCollectingData = true;
            BtnPressureValue = context.ReadValue<float>();
        }

        private void OnRightHandFingerToneBtnReleased(InputAction.CallbackContext context) {
            BtnPressureValue = 0f;
            scO.isRightFingerToneCollectingData = false;
        }
        
    }
}