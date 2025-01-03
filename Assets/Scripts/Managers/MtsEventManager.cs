using ScriptableObjects;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Managers {
    public class MtsEventManager : Singleton<MtsEventManager> {
        
        public delegate void RightHandMotionTrackingAction(BodyPart bodyPart);
        public event RightHandMotionTrackingAction OnRightHandBtnPressed;
        public delegate void LeftHandMotionTrackingAction(BodyPart bodyPart);
        public event LeftHandMotionTrackingAction OnLeftHandBtnPressed;
        public delegate void HeadMotionTrackingAction(BodyPart bodyPart);
        public event HeadMotionTrackingAction OnHeadBtnPressed;
        
        public delegate void ButtonReleasedAction();
        public event ButtonReleasedAction OnBtnReleased;
        
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
            _inputActions.XRIRightInteraction.UIPress.canceled += MotionTrackingReleased;
        }

        private void OnDisable() {
            _inputActions.XRIRightInteraction.UIPress.performed -= RightHandMotionTrackingBtnPressed;
            _inputActions.XRILeftInteraction.UIPress.performed -= LeftHandMotionTrackingBtnPressed;
            _inputActions.XRIRightInteraction.UIPress.performed -= MotionTrackingReleased;
            _inputActions.XRIRightInteraction.UIPress.Disable();
            _inputActions.XRILeftInteraction.UIPress.Disable();
        }

        private void RightHandMotionTrackingBtnPressed(InputAction.CallbackContext context) {
            OnRightHandBtnPressed?.Invoke(BodyPart.RightHand);
        }
        
        private void LeftHandMotionTrackingBtnPressed(InputAction.CallbackContext context) {
            OnLeftHandBtnPressed?.Invoke(BodyPart.LeftHand);
        }

        private void MotionTrackingReleased(InputAction.CallbackContext context) {
            OnBtnReleased?.Invoke();
        }
    }
}