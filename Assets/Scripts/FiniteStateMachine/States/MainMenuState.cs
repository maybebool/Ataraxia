using FiniteStateMachine.Structures;
using UnityEngine;

namespace FiniteStateMachine.States {
    public class MainMenuState : BaseState {
        private readonly GameObject[] _objectsToEnable;

        public MainMenuState(GameObject[] objectsToEnable) {
            _objectsToEnable = objectsToEnable;
        }

        public override void OnEnter() {
            foreach (var go in _objectsToEnable) {
                if (go) go.SetActive(true);
            }
            Debug.Log("MainMenuState -> OnEnter");
        }

        public override void OnExit() {
            foreach (var go in _objectsToEnable) {
                if (go) go.SetActive(false);
            }
            Debug.Log("MainMenuState -> OnExit");
        }
    }

}