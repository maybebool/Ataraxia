using UnityEngine;

namespace StateMachines.States {
    public class SceneState : BaseState {
        private GameObject[] objects;

        public SceneState(GameObject[] objects) {
            this.objects = objects;
        }

        public override void OnEnter() {
            // Activate only relevant GOs for this scene
            foreach (var go in objects) {
                go.SetActive(true);
            }
        }

        public override void OnExit() {
            // Deactivate them on exit
            foreach (var go in objects) {
                go.SetActive(false);
            }
        }
    }
}