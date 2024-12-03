using UnityEngine.InputSystem;

namespace Managers {
    public class MtsEventManager : Singleton<MtsEventManager> {

        // Events
        public delegate void ButtonPressedAction();
        public event ButtonPressedAction OnButtonPressed;

        public delegate void ButtonReleasedAction();
        public event ButtonReleasedAction OnButtonReleased;

        // Input action asset
        private XRIDefaultInputActions _inputActions;

        private void Awake() {
            _inputActions = new XRIDefaultInputActions();
        }

        private void OnEnable()
        {
            _inputActions.XRIRightLocomotion.TeleportMode.Enable();

            // Register callbacks for button press and release
            _inputActions.XRIRightLocomotion.TeleportMode.performed += HandleButtonPressed;
            _inputActions.XRIRightLocomotion.TeleportMode.canceled += HandleButtonReleased;
        }

        private void OnDisable()
        {
            _inputActions.XRIRightLocomotion.TeleportMode.performed -= HandleButtonPressed;
            _inputActions.XRIRightLocomotion.TeleportMode.performed -= HandleButtonReleased;

            _inputActions.XRIRightLocomotion.TeleportMode.Disable();
        }

        private void HandleButtonPressed(InputAction.CallbackContext context)
        {
            OnButtonPressed?.Invoke();
        }

        private void HandleButtonReleased(InputAction.CallbackContext context)
        {
            OnButtonReleased?.Invoke();
        }
    }
}