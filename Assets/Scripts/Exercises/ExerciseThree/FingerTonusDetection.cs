using UnityEngine;
using UnityEngine.InputSystem;

namespace Exercises.ExerciseThree {
    public class FingerTonusDetection : MonoBehaviour{
        public InputActionReference triggerAction;
        private XRIDefaultInputActions value;


        private void Awake() {
            value = new XRIDefaultInputActions();
        }

        private void OnEnable() {
            triggerAction.action.Enable();
            triggerAction.action.performed += OnTriggerPerformed;
        }

        private void OnDisable() {
            triggerAction.action.performed -= OnTriggerPerformed;
        }
        
        private void OnTriggerPerformed(InputAction.CallbackContext context) {
            var triggerValue = context.ReadValue<float>();
            Debug.Log($"Trigger pressed with a value of: {triggerValue}");
        }
    }
}