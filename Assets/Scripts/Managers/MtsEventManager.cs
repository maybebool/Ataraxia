using ScriptableObjects;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Managers {
    public class MtsEventManager : Singleton<MtsEventManager> {
        
        public delegate void ButtonPressedAction(BodyPart bodyPart);
        public event ButtonPressedAction OnButtonPressed;
        
        public delegate void ButtonReleasedAction();
        public event ButtonReleasedAction OnButtonReleased;
        
        [SerializeField] private DataContainer dataContainer;
        private XRIDefaultInputActions _inputActions;

        private void Awake() {
            _inputActions = new XRIDefaultInputActions();
        }

        private void OnEnable() {
            _inputActions.XRIRightInteraction.UIPress.Enable();
            _inputActions.XRIRightInteraction.UIPress.performed += HandleButtonPressed;
            _inputActions.XRIRightInteraction.UIPress.canceled += HandleButtonReleased;
        }

        private void OnDisable() {
            _inputActions.XRIRightInteraction.UIPress.performed -= HandleButtonPressed;
            _inputActions.XRIRightInteraction.UIPress.performed -= HandleButtonReleased;
            _inputActions.XRIRightInteraction.UIPress.Disable();
        }

        private void HandleButtonPressed(InputAction.CallbackContext context) {
            OnButtonPressed?.Invoke(BodyPart.RightHand);
        }

        private void HandleButtonReleased(InputAction.CallbackContext context) {
            OnButtonReleased?.Invoke();
        }
    }
}