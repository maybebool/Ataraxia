using FiniteStateMachine.States;
using FiniteStateMachine.Structures;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FiniteStateMachine {
    public class SceneStateMachine : MonoBehaviour {
        [Header("Main Menu Objects")]
        [SerializeField] private GameObject[] mainMenuObjects;

        [Header("Exercise1 Objects")] 
        [SerializeField] private GameObject[] exercise1Objects;

        [Header("Exercise2 Objects")] 
        [SerializeField] private GameObject[] exercise2Objects;

        [Header("Exercise3 Objects")] 
        [SerializeField] private GameObject[] exercise3Objects;
        
        private Structures.StateMachine _sceneFsm;
        private MainMenuState _mainMenuState;
        private Exercise1State _exercise1State;
        private Exercise2State _exercise2State;
        private Exercise3State _exercise3State;

        private void Awake() {
            
            _sceneFsm = new Structures.StateMachine();
            _mainMenuState = new MainMenuState(mainMenuObjects);
            _exercise1State = new Exercise1State(exercise1Objects);
            _exercise2State = new Exercise2State(exercise2Objects);
            _exercise3State = new Exercise3State(exercise3Objects);
            
            // Register states, otherwise Dictionary will throw an exception at start
            _sceneFsm.RegisterState(_mainMenuState);
            _sceneFsm.RegisterState(_exercise1State);
            _sceneFsm.RegisterState(_exercise2State);
            _sceneFsm.RegisterState(_exercise3State);
        }

        private void OnEnable() {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDestroy() {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
            SwitchToSceneState(scene.name);
        }

        private void SwitchToSceneState(string loadedSceneName) {
            switch (loadedSceneName) {
                case "MainMenu":
                    _sceneFsm.SetState(_mainMenuState);
                    break;
                case "Exercise1":
                    _sceneFsm.SetState(_exercise1State);
                    break;
                case "Exercise2":
                    _sceneFsm.SetState(_exercise2State);
                    break;
                case "Exercise3":
                    _sceneFsm.SetState(_exercise3State);
                    break;
                default:
                    Debug.LogWarning($"Scene '{loadedSceneName}' not recognized in SceneStateMachine.");
                    break;
            }
        }
    }
}