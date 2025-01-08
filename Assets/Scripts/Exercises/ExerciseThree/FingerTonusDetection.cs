using Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Exercises.ExerciseThree {
    public class FingerTonusDetection : MonoBehaviour{

        private void OnEnable() {
            if (MtsEventManager.Instance != null) {
                MtsEventManager.Instance.OnLeftHandFingerToneBtnPressed += OnLeftHandFingerToneBtnPressed;
                MtsEventManager.Instance.OnLeftHandFingerToneBtnReleased += OnLeftHandFingerToneBtnReleased;
            }
        }

        private void OnDisable() {
            if (MtsEventManager.Instance != null) {
                MtsEventManager.Instance.OnLeftHandFingerToneBtnPressed -= OnLeftHandFingerToneBtnPressed;
                MtsEventManager.Instance.OnLeftHandFingerToneBtnReleased -= OnLeftHandFingerToneBtnReleased;
            }
        }
        
        private void OnLeftHandFingerToneBtnPressed(InputAction.CallbackContext context) {
            var triggerValue = context.ReadValue<float>();
            Debug.Log($"Trigger pressed with a value of: {triggerValue}");
        }
        
        private void OnLeftHandFingerToneBtnReleased(InputAction.CallbackContext context) {
            Debug.Log("Left hand finger tonus btn released");
        }
    }
}