using ScriptableObjects;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Managers {
    public class MtsEventManager : Singleton<MtsEventManager> {
        
        [SerializeField] private DataContainer dataContainer;
        private XRIDefaultInputActions _inputActions;

        #region Delegates/Events

        // Right Hand
        public delegate void RightHandMotionTrackingAction();
        public event RightHandMotionTrackingAction OnRightHandBtnPressed;
        public delegate void RightHandButtonReleasedAction();
        public event RightHandButtonReleasedAction OnRightHandBtnReleased;


        // Left Hand
        public delegate void LeftHandMotionTrackingAction();
        public event LeftHandMotionTrackingAction OnLeftHandBtnPressed;
        public delegate void LeftHandButtonReleasedAction();
        public event LeftHandButtonReleasedAction OnLeftHandBtnReleased;


        // Head
        public delegate void HeadMotionTrackingAction();
        public event HeadMotionTrackingAction OnHeadActionActivated;
        public delegate void HeadMotionTrackingActionDeactivated();
        public event HeadMotionTrackingActionDeactivated OnHeadActionDeactivated;


        // Right Leg
        public delegate void RightLegMotionTrackingAction();
        public event RightLegMotionTrackingAction OnRightLegBtnPressed;
        public delegate void RightLegButtonReleasedAction();
        public event RightLegButtonReleasedAction OnRightLegBtnReleased;


        // Left Leg
        public delegate void LeftLegMotionTrackingAction();
        public event LeftLegMotionTrackingAction OnLeftLegBtnPressed;
        public delegate void LeftLegButtonReleasedAction();
        public event LeftLegButtonReleasedAction OnLeftLegBtnReleased;
        
        
        // Right Hand Finger Tonus
        public delegate void RightHandFingerTonusAction();
        public event RightHandFingerTonusAction OnRightHandFingerTonusBtnPressed;
        public delegate void RightHandFingerTonusActionDeactivated();
        public event RightHandFingerTonusActionDeactivated OnRightHandFingerTonusBtnReleased;
        

        #endregion

        private void Awake() {
            _inputActions = new XRIDefaultInputActions();
        }

        private void OnEnable() {
            EnableRightHandEvents();
            EnableLeftHandEvents();
            EnableHeadEvents();
            EnableRightLegEvents();
            EnableLeftLegEvents();
            EnableRightHandFingerEvents();
        }

        private void OnDisable() {
            DisableRightHandEvents();
            DisableLeftHandEvents();
            DisableHeadEvents();
            DisableRightLegEvents();
            DisableLeftLegEvents();
            DisableRightHandFingerEvents();
        }

        #region Right Hand Bindings

        private void EnableRightHandEvents() {
            _inputActions.XRIRightInteraction.UIPress.Enable();
            _inputActions.XRIRightInteraction.UIPress.performed += RightHandMotionTrackingBtnPressed;
            _inputActions.XRIRightInteraction.UIPress.canceled += RightHandMotionTrackingBtnReleased;
        }

        private void DisableRightHandEvents() {
            _inputActions.XRIRightInteraction.UIPress.performed -= RightHandMotionTrackingBtnPressed;
            _inputActions.XRIRightInteraction.UIPress.canceled -= RightHandMotionTrackingBtnReleased;
            _inputActions.XRIRightInteraction.UIPress.Disable();
        }

        private void RightHandMotionTrackingBtnPressed(InputAction.CallbackContext context) {
            OnRightHandBtnPressed?.Invoke();
        }

        private void RightHandMotionTrackingBtnReleased(InputAction.CallbackContext context) {
            OnRightHandBtnReleased?.Invoke();
        }

        #endregion

        #region Left Hand Bindings

        private void EnableLeftHandEvents() {
            _inputActions.XRILeftInteraction.UIPress.Enable();
            _inputActions.XRILeftInteraction.UIPress.performed += LeftHandMotionTrackingBtnPressed;
            _inputActions.XRILeftInteraction.UIPress.canceled += LeftHandMotionTrackingBtnReleased;
        }

        private void DisableLeftHandEvents() {
            _inputActions.XRILeftInteraction.UIPress.performed -= LeftHandMotionTrackingBtnPressed;
            _inputActions.XRILeftInteraction.UIPress.canceled -= LeftHandMotionTrackingBtnReleased;
            _inputActions.XRILeftInteraction.UIPress.Disable();
        }

        private void LeftHandMotionTrackingBtnPressed(InputAction.CallbackContext context) {
            OnLeftHandBtnPressed?.Invoke();
        }

        private void LeftHandMotionTrackingBtnReleased(InputAction.CallbackContext context) {
            OnLeftHandBtnReleased?.Invoke();
        }

        #endregion

        #region Head Bindings

        private void EnableHeadEvents() {
            _inputActions.XRIHead.HeadToggle.Enable();
            _inputActions.XRIHead.HeadToggle.performed += HeadMotionActivated;
            _inputActions.XRIHead.HeadToggle.canceled += HeadMotionDeactivated;
        }

        private void DisableHeadEvents() {
            _inputActions.XRIHead.HeadToggle.performed -= HeadMotionActivated;
            _inputActions.XRIHead.HeadToggle.canceled -= HeadMotionDeactivated;
            _inputActions.XRIHead.HeadToggle.Disable();
        }

        private void HeadMotionActivated(InputAction.CallbackContext context) {
            OnHeadActionActivated?.Invoke();
        }

        private void HeadMotionDeactivated(InputAction.CallbackContext context) {
            OnHeadActionDeactivated?.Invoke();
        }

        #endregion

        #region Right Leg Bindings

        private void EnableRightLegEvents() {
            _inputActions.XRIRightInteraction.UIPress.performed += RightLegMotionTrackingBtnPressed;
            _inputActions.XRIRightInteraction.UIPress.canceled += RightLegMotionTrackingBtnReleased;
        }

        private void DisableRightLegEvents() {
            _inputActions.XRIRightInteraction.UIPress.performed -= RightLegMotionTrackingBtnPressed;
            _inputActions.XRIRightInteraction.UIPress.canceled -= RightLegMotionTrackingBtnReleased;
        }

        private void RightLegMotionTrackingBtnPressed(InputAction.CallbackContext context) {
            OnRightLegBtnPressed?.Invoke();
        }

        private void RightLegMotionTrackingBtnReleased(InputAction.CallbackContext context) {
            OnRightLegBtnReleased?.Invoke();
        }

        #endregion

        #region Left Leg Bindings

        private void EnableLeftLegEvents() {
            _inputActions.XRILeftInteraction.UIPress.performed += LeftLegMotionTrackingBtnPressed;
            _inputActions.XRILeftInteraction.UIPress.canceled += LeftLegMotionTrackingBtnReleased;
        }

        private void DisableLeftLegEvents() {
            _inputActions.XRILeftInteraction.UIPress.performed -= LeftLegMotionTrackingBtnPressed;
            _inputActions.XRILeftInteraction.UIPress.canceled -= LeftLegMotionTrackingBtnReleased;
        }

        private void LeftLegMotionTrackingBtnPressed(InputAction.CallbackContext context) {
            OnLeftLegBtnPressed?.Invoke();
        }

        private void LeftLegMotionTrackingBtnReleased(InputAction.CallbackContext context) {
            OnLeftLegBtnReleased?.Invoke();
        }

        #endregion

        #region Right Hand Finger Tonus Binding

        private void EnableRightHandFingerEvents() {
            _inputActions.XRIRightInteraction.UIPress.Enable();
            _inputActions.XRIRightInteraction.UIPress.performed += RightHandFingerTonusBtnPressed;
            _inputActions.XRIRightInteraction.UIPress.canceled += RightHandFingerTonusBtnReleased;
        }

        private void DisableRightHandFingerEvents() {
            _inputActions.XRIRightInteraction.UIPress.performed -= RightHandFingerTonusBtnPressed;
            _inputActions.XRIRightInteraction.UIPress.canceled -= RightHandFingerTonusBtnReleased;
            _inputActions.XRIRightInteraction.UIPress.Disable();
        }

        private void RightHandFingerTonusBtnPressed(InputAction.CallbackContext context) {
            OnRightHandFingerTonusBtnPressed?.Invoke();
        }

        private void RightHandFingerTonusBtnReleased(InputAction.CallbackContext context) {
            OnRightHandFingerTonusBtnReleased?.Invoke();
        }
        

        #endregion
    }
}