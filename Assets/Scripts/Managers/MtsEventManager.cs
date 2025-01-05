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

        public event HeadMotionTrackingAction OnHeadActionActivated;
        
        
        public delegate void HeadMotionTrackingActionDeactivated();

        public event HeadMotionTrackingActionDeactivated OnHeadActionDeactivated;
        
        

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
            _inputActions.XRIHead.HeadToggle.Enable();

            _inputActions.XRIRightInteraction.UIPress.performed += RightHandMotionTrackingBtnPressed;
            _inputActions.XRILeftInteraction.UIPress.performed += LeftHandMotionTrackingBtnPressed;
            _inputActions.XRIHead.HeadToggle.performed += HeadMotionActivated;
            
            _inputActions.XRIRightInteraction.UIPress.canceled += RightHandMotionTrackingBtnReleased;
            _inputActions.XRILeftInteraction.UIPress.canceled += LeftHandMotionTrackingBtnReleased;
            _inputActions.XRIHead.HeadToggle.canceled += HeadMotionDeactivated;
            
        }

        private void OnDisable() {
            _inputActions.XRIRightInteraction.UIPress.performed -= RightHandMotionTrackingBtnPressed;
            _inputActions.XRILeftInteraction.UIPress.performed -= LeftHandMotionTrackingBtnPressed;
            _inputActions.XRIHead.HeadToggle.performed += HeadMotionActivated;

            _inputActions.XRIRightInteraction.UIPress.performed -= RightHandMotionTrackingBtnReleased;
            _inputActions.XRILeftInteraction.UIPress.performed -= LeftHandMotionTrackingBtnReleased;
            _inputActions.XRIHead.HeadToggle.canceled += HeadMotionDeactivated;

            _inputActions.XRIRightInteraction.UIPress.Disable();
            _inputActions.XRILeftInteraction.UIPress.Disable();
            _inputActions.XRIHead.HeadToggle.Disable();
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
        
        private void HeadMotionActivated(InputAction.CallbackContext context) {
            OnHeadActionActivated?.Invoke();
        }

        private void HeadMotionDeactivated(InputAction.CallbackContext context) {
            OnHeadActionDeactivated?.Invoke();
        }
        

    }
}