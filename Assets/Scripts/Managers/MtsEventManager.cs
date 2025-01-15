using Audio;
using GameUI;
using SceneHandling;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Managers {
    public class MtsEventManager : Singleton<MtsEventManager> {
        
        [SerializeField] private DataContainer dataContainer;
        private XRIDefaultInputActions _inputActions;
        private int _collectedTargetsCount = 0;
        

        #region MTA Events

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
        public delegate void RightHandFingerToneAction(InputAction.CallbackContext context);
        public event RightHandFingerToneAction OnRightHandFingerToneBtnPressed;
        public delegate void RightHandFingerToneActionDeactivated(InputAction.CallbackContext context);
        public event RightHandFingerToneActionDeactivated OnRightHandFingerToneBtnReleased;
        
        // Left Hand Finger Tonus
        public delegate void LeftHandFingerToneAction(InputAction.CallbackContext context);
        public event LeftHandFingerToneAction OnLeftHandFingerToneBtnPressed;
        public delegate void LeftHandFingerToneActionDeactivated(InputAction.CallbackContext context);
        public event LeftHandFingerToneActionDeactivated OnLeftHandFingerToneBtnReleased;
        

        #endregion

        #region Scene Events
        public delegate void MainMenuLoadedAction();
        public event MainMenuLoadedAction OnMainMenuLoaded;

        public delegate void SceneOnExercise1Action();
        public event SceneOnExercise1Action OnExercise1;
        public delegate void TenTargetsCollectedAction();
        public event TenTargetsCollectedAction OnTenTargetsCollected;
        private TenTargetsCollectedAction _onTenTargetsCollectedHandler;

        public delegate void SceneOnExercise2Action();
        public event SceneOnExercise2Action OnExercise2;

        public delegate void SceneOnExercise3Action();
        public event SceneOnExercise3Action OnExercise3;
        

        #endregion

        private void Awake() {
            _inputActions = new XRIDefaultInputActions();
        }

        private void OnEnable() {
            SceneManager.sceneLoaded += OnSceneLoaded;
            EnableTenTargetsCollectedHandler();
            EnableRightHandEvents();
            EnableLeftHandEvents();
            EnableHeadEvents();
            EnableRightLegEvents();
            EnableLeftLegEvents();
            EnableRightHandFingerEvents();
            EnableLeftHandFingerEvents();
        }


        private void OnDisable() {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            DisableTenTargetsCollected();
            DisableRightHandEvents();
            DisableLeftHandEvents();
            DisableHeadEvents();
            DisableRightLegEvents();
            DisableLeftLegEvents();
            DisableRightHandFingerEvents();
            DisableLeftHandFingerEvents();
        }

        #region Exercise 1 Events
        private void EnableTenTargetsCollectedHandler() {
            _onTenTargetsCollectedHandler = () => LoadNewExercise(SceneNames.Exercise2);
            OnTenTargetsCollected += _onTenTargetsCollectedHandler;
        }

        private void DisableTenTargetsCollected() {
            if (_onTenTargetsCollectedHandler != null) {
                OnTenTargetsCollected -= _onTenTargetsCollectedHandler;
            }
        }
        
        #endregion

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

        #region Right Hand Finger Tone Binding

        private void EnableRightHandFingerEvents() {
            _inputActions.XRIRightInteraction.UIPressValue.Enable();
            _inputActions.XRIRightInteraction.UIPressValue.performed += RightHandFingerTonusBtnPressed;
            _inputActions.XRIRightInteraction.UIPressValue.canceled += RightHandFingerTonusBtnReleased;
        }

        private void DisableRightHandFingerEvents() {
            _inputActions.XRIRightInteraction.UIPressValue.performed -= RightHandFingerTonusBtnPressed;
            _inputActions.XRIRightInteraction.UIPressValue.canceled -= RightHandFingerTonusBtnReleased;
            _inputActions.XRIRightInteraction.UIPressValue.Disable();
        }

        private void RightHandFingerTonusBtnPressed(InputAction.CallbackContext context) {
            OnRightHandFingerToneBtnPressed?.Invoke(context);
        }

        private void RightHandFingerTonusBtnReleased(InputAction.CallbackContext context) {
            OnRightHandFingerToneBtnReleased?.Invoke(context);
        }
        

        #endregion

        #region Left Hand Finger Tone Binding

        private void EnableLeftHandFingerEvents() {
            _inputActions.XRILeftInteraction.UIPressValue.Enable();
            _inputActions.XRILeftInteraction.UIPressValue.performed += LeftHandFingerTonusBtnPressed;
            _inputActions.XRILeftInteraction.UIPressValue.canceled += LeftHandFingerTonusBtnReleased;
        }

        private void DisableLeftHandFingerEvents() {
            _inputActions.XRILeftInteraction.UIPressValue.performed -= LeftHandFingerTonusBtnPressed;
            _inputActions.XRILeftInteraction.UIPressValue.canceled -= LeftHandFingerTonusBtnReleased;
            _inputActions.XRILeftInteraction.UIPressValue.Disable();
        }

        private void LeftHandFingerTonusBtnPressed(InputAction.CallbackContext context) {
            OnLeftHandFingerToneBtnPressed?.Invoke(context);
        }

        private void LeftHandFingerTonusBtnReleased(InputAction.CallbackContext context) {
            OnLeftHandFingerToneBtnReleased?.Invoke(context);
        }
        
        #endregion

        #region Methods
        
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
            switch (scene.name) {
                case nameof(SceneNames.MainMenu):
                    OnMainMenuLoaded?.Invoke();
                    break;
                case nameof(SceneNames.Exercise1):
                    OnExercise1?.Invoke();
                    break;
                case nameof(SceneNames.Exercise2):
                    OnExercise2?.Invoke();
                    break;
                case nameof(SceneNames.Exercise3):
                    OnExercise3?.Invoke();
                    break;
            }
        }
        
        private void LoadNewExercise(SceneNames sceneIndex) {
            SceneLoader.Instance.LoadNewScene(sceneIndex);
            AudioController.Instance.PlayAudioClip(4, 2);
        }

        public void IncrementTargetCount() {
            _collectedTargetsCount++;

            if (_collectedTargetsCount >= 3) {
                OnTenTargetsCollected?.Invoke();
            }
        }

        #endregion
    }
}