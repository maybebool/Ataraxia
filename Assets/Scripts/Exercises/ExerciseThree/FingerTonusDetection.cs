using Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Exercises.ExerciseThree {
    public class FingerTonusDetection : MonoBehaviour {
        [Header("Scaling Settings (Same Logic)")]
        public float minHeight = 0.1f;

        public float maxHeight = 5f;

        [Tooltip("How fast to smoothly interpolate each frame")]
        public float interpolationSpeed = 1.5f;

        // This is our [0..1] progress value, driven by the trigger
        private float btnPressureValue = 0f;

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

        private void Update() {
            // Same "growing/shrinking" logic, but user-driven targetT
            var currentY = transform.localScale.y;
            var desiredY = Mathf.Lerp(minHeight, maxHeight, btnPressureValue);

            // Smoothly interpolate
            var newY = Mathf.Lerp(currentY, desiredY, Time.deltaTime * interpolationSpeed);
            var delta = newY - currentY;

            // Shift upward half the delta to grow "upwards"
            transform.localPosition += new Vector3(0, delta * 0.5f, 0);

            transform.localScale = new Vector3(transform.localScale.x, newY, transform.localScale.z);
        }

        private void OnLeftHandFingerToneBtnPressed(InputAction.CallbackContext context) {
            btnPressureValue = context.ReadValue<float>();
            Debug.Log($"Trigger pressed with a value of: {btnPressureValue}");
        }

        private void OnLeftHandFingerToneBtnReleased(InputAction.CallbackContext context) {
            btnPressureValue = 0f;
            Debug.Log("Left hand finger tonus btn released");
        }
    }
}