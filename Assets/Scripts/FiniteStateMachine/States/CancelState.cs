using UnityEngine;
using UnityEngine.SceneManagement;

namespace FiniteStateMachine.States {
    public class CancelState : BaseState {
        private readonly string _mainMenuScene;

        public CancelState(string mainMenuSceneName) {
            _mainMenuScene = mainMenuSceneName;
        }

        public override void OnEnter() {
            Debug.Log("Entering Cancel (Quit) State -> Loading Main Menu");
            SceneManager.LoadScene(_mainMenuScene);
        }
    }
}