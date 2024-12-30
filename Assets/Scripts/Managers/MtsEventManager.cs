using GameUI;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Managers {
    public class MtsEventManager : Singleton<MtsEventManager> {
        
        public delegate void ButtonPressedAction();
        public event ButtonPressedAction OnButtonPressed;
        
        public delegate void ButtonReleasedAction();
        public event ButtonReleasedAction OnButtonReleased;
        
        public delegate void SceneBuildIndexChanged(int buildIndex, bool isActive);
        public event SceneBuildIndexChanged OnSceneActiveChanged;
        
        [SerializeField] private DataContainer dataContainer;
        
        private XRIDefaultInputActions _inputActions;

        private void Awake() {
            _inputActions = new XRIDefaultInputActions();
        }

        private void OnEnable() {
            _inputActions.XRIRightLocomotion.TeleportMode.Enable();
            _inputActions.XRIRightLocomotion.TeleportMode.performed += HandleButtonPressed;
            _inputActions.XRIRightLocomotion.TeleportMode.canceled += HandleButtonReleased;
        }

        private void OnDisable() {
            _inputActions.XRIRightLocomotion.TeleportMode.performed -= HandleButtonPressed;
            _inputActions.XRIRightLocomotion.TeleportMode.performed -= HandleButtonReleased;
            _inputActions.XRIRightLocomotion.TeleportMode.Disable();
        }

        private void HandleButtonPressed(InputAction.CallbackContext context) {
            OnButtonPressed?.Invoke();
        }

        private void HandleButtonReleased(InputAction.CallbackContext context) {
            OnButtonReleased?.Invoke();
        }
        
        
        public void SetSceneActive(int buildIndex, bool isActive) {
            // Fire the event
            OnSceneActiveChanged?.Invoke(buildIndex, isActive);
        }
    }
}