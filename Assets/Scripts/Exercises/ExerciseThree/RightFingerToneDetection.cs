using Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Exercises.ExerciseThree {
    public class RightFingerToneDetection : MTAToneBase{
        protected override float NewYAxisScaleValue { 
            get => scO.rightPlayerObjectHeight; 
            set => scO.rightPlayerObjectHeight = value; }
        protected override float BtnPressureValue { get; set; } = 0;
        protected override bool IsCollectingData {
            get => scO.isRightFingerToneCollectingData;
            set => scO.isRightFingerToneCollectingData = value ;
        }

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
            BtnPressureValue = context.ReadValue<float>();
            StartToneCollection();
        }

        private void OnRightHandFingerToneBtnReleased(InputAction.CallbackContext context) {
            BtnPressureValue = 0f;
            StopToneCollection();
        }
        
    }
}