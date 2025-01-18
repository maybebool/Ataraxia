using UnityEngine;

namespace StateMachines.States {
    public class PausedState : BaseState {
        private GameObject pauseMenu;

        public PausedState(GameObject pauseMenu) {
            this.pauseMenu = pauseMenu;
        }

        public override void OnEnter() {
            // Freeze the game
            Time.timeScale = 0f;

            // Show Pause Menu UI
            if (pauseMenu != null) {
                pauseMenu.SetActive(true);
            }
        
            Debug.Log("Entering Paused State");
        }

        public override void OnExit() {
            // Hide Pause Menu if we exit the paused state
            if (pauseMenu != null) {
                pauseMenu.SetActive(false);
            }
        }
    }
}