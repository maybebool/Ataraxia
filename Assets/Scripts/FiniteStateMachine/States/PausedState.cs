using UnityEngine;

namespace FiniteStateMachine.States {
    public class PausedState : BaseState {
        private readonly GameObject _pauseMenu;

        public PausedState(GameObject pauseMenu) {
            _pauseMenu = pauseMenu;
        }

        public override void OnEnter() {
            Time.timeScale = 0f;
            if (_pauseMenu != null) {
                _pauseMenu.SetActive(true);
            }
        
            Debug.Log("Entering Paused State");
        }

        public override void OnExit() {
            if (_pauseMenu != null) {
                _pauseMenu.SetActive(false);
            }
        }
    }
}