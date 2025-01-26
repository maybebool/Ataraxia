using FiniteStateMachine.Structures;
using UnityEngine;

namespace FiniteStateMachine.States {
    public class PausedState : BaseState {
        private readonly GameObject _pauseMenu;
        private readonly TremorDetectionStateMachine _manager;
        public PausedState(GameObject pauseMenu, TremorDetectionStateMachine manager) {
            _pauseMenu = pauseMenu;
            _manager = manager;
        }

        public override void OnEnter() {
            Time.timeScale = 0f;
            if (_pauseMenu) {
                _pauseMenu.SetActive(true);
            }
            
            _manager.HandlePauseEntered();
        }

        public override void OnExit() {
            if (_pauseMenu) {
                _pauseMenu.SetActive(false);
            }
        }
    }
}