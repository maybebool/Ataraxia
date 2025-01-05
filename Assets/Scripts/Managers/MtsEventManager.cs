using ScriptableObjects;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Managers {
    public class MtsEventManager : Singleton<MtsEventManager> {

        public delegate void RightHandMotionTrackingAction();

        public event RightHandMotionTrackingAction OnRightHandBtnPressed;

        public delegate void LeftHandMotionTrackingAction();

        public event LeftHandMotionTrackingAction OnLeftHandBtnPressed;

        public delegate void HeadMotionTrackingAction();

        public event HeadMotionTrackingAction OnHeadBtnPressed;

        public delegate void RightHandButtonReleasedAction();

        public event RightHandButtonReleasedAction OnRightHandBtnReleased;

        public delegate void LeftHandButtonReleasedAction();

        public event LeftHandButtonReleasedAction OnLeftHandBtnReleased;

        [SerializeField] private DataContainer dataContainer;
        private XRIDefaultInputActions _inputActions;

        private void Awake() {
            _inputActions = new XRIDefaultInputActions();
        }

        private void OnEnable() {
            _inputActions.XRIRightInteraction.UIPress.Enable();
            _inputActions.XRILeftInteraction.UIPress.Enable();

            _inputActions.XRIRightInteraction.UIPress.performed += RightHandMotionTrackingBtnPressed;
            _inputActions.XRILeftInteraction.UIPress.performed += LeftHandMotionTrackingBtnPressed;


            _inputActions.XRIRightInteraction.UIPress.canceled += RightHandMotionTrackingBtnReleased;
            _inputActions.XRILeftInteraction.UIPress.canceled += LeftHandMotionTrackingBtnReleased;


        }

        private void OnDisable() {
            _inputActions.XRIRightInteraction.UIPress.performed -= RightHandMotionTrackingBtnPressed;
            _inputActions.XRILeftInteraction.UIPress.performed -= LeftHandMotionTrackingBtnPressed;

            _inputActions.XRIRightInteraction.UIPress.performed -= RightHandMotionTrackingBtnReleased;
            _inputActions.XRILeftInteraction.UIPress.performed -= LeftHandMotionTrackingBtnReleased;

            _inputActions.XRIRightInteraction.UIPress.Disable();
            _inputActions.XRILeftInteraction.UIPress.Disable();
        }

        private void RightHandMotionTrackingBtnPressed(InputAction.CallbackContext context) {
            OnRightHandBtnPressed?.Invoke();
        }

        private void RightHandMotionTrackingBtnReleased(InputAction.CallbackContext context) {
            OnRightHandBtnReleased?.Invoke();
        }

        private void LeftHandMotionTrackingBtnPressed(InputAction.CallbackContext context) {
            OnLeftHandBtnPressed?.Invoke();
        }

        private void LeftHandMotionTrackingBtnReleased(InputAction.CallbackContext context) {
            OnLeftHandBtnReleased?.Invoke();
        }

    }
}