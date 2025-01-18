using UnityEngine;

namespace FiniteStateMachine.States {
    public class Exercise1State : BaseState {
        private readonly GameObject[] _objectsToEnable;

        public Exercise1State(GameObject[] objectsToEnable) {
            _objectsToEnable = objectsToEnable;
        }

        public override void OnEnter() {
            foreach (var go in _objectsToEnable) {
                if (go) go.SetActive(true);
            }
            Debug.Log("Exercise1State -> OnEnter");
        }

        public override void OnExit() {
            foreach (var go in _objectsToEnable) {
                if (go) go.SetActive(false);
            }
            Debug.Log("Exercise1State -> OnExit");
        }
    }
}