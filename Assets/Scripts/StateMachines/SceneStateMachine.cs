using System.Collections.Generic;
using StateMachines.States;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace StateMachines {
    public class SceneStateMachine : MonoBehaviour {
        [SerializeField] private List<SceneBinding> sceneBindings;

        private StateMachine sceneFsm;
        private Dictionary<int, SceneState> bindingStates;

        private SceneState currentSceneState;

        private void Awake() {
            sceneFsm = new StateMachine();
            bindingStates = new Dictionary<int, SceneState>();

            // Create SceneStates from each binding
            foreach (var binding in sceneBindings) {
                var sceneState = new SceneState(binding.sceneObjects);
                bindingStates.Add(binding.buildIndex, sceneState);
            }
        }

        private void OnEnable() {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable() {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void Start() {
            // If something is loaded at start, we force a check
            int currentBuildIndex = SceneManager.GetActiveScene().buildIndex;
            TransitionToSceneState(currentBuildIndex);
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadMode) {
            // Scene loaded => transition in state machine
            TransitionToSceneState(scene.buildIndex);
        }

        private void TransitionToSceneState(int buildIndex) {
            // If we don't have a state for this, just do nothing
            if (!bindingStates.TryGetValue(buildIndex, out var newState)) {
                return;
            }

            // Actually transition inside the fsm
            sceneFsm.SetState(newState);
        }

        private void Update() {
            // Keep the scene fsm updated if we had transitions
            sceneFsm.Update();
        }

        private void FixedUpdate() {
            sceneFsm.FixedUpdate();
        }
    }
}