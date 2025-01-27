using System.Collections;
using System.Collections.Generic;
using FiniteStateMachine.States;
using FiniteStateMachine.Structures;
using ScriptableObjects;
using UnityEngine;

namespace FiniteStateMachine {
    public class TremorDetectionStateMachine : MonoBehaviour {
        [SerializeField] private DataContainer dataContainer;
        [SerializeField] private GameObject pauseMenu; 
        [SerializeField] private string mainMenuSceneName = "MainMenu";

        private Structures.StateMachine _fsm;
        private RunningState _runningState;
        private PausedState _pausedState;
        private CancelState _cancelState;

        private void Awake() {
            _fsm = new Structures.StateMachine();
        
            _runningState = new RunningState();
            _pausedState = new PausedState(pauseMenu, this);
            _cancelState = new CancelState();

            // workaround needed, higher values unreachable because line graph resets value 
            // also calculatedTremorIntensity not implemented in line graph yet
            
            dataContainer.calculatedTremorIntensity = 0.0f;
            dataContainer.isPausedRecently = false;
            // Define transitions with updated predicates
            // 1. Running -> Paused if intensity in [8, 9.8) and not paused recently
            _fsm.AddTransition(_runningState, _pausedState, new FuncPredicate(() =>
                dataContainer.calculatedTremorIntensity >= 8.0f &&
                dataContainer.calculatedTremorIntensity < 9.8f &&
                !dataContainer.isPausedRecently));

            // 2. Running -> Cancel if intensity in [8, 9.8) and paused recently
            _fsm.AddTransition(_runningState, _cancelState, new FuncPredicate(() =>
                dataContainer.calculatedTremorIntensity >= 8.0f &&
                dataContainer.calculatedTremorIntensity < 9.8f &&
                dataContainer.isPausedRecently));

            // Running -> Cancel if intensity >= 9.8
            _fsm.AddTransition(_runningState, _cancelState, new FuncPredicate(() =>
                dataContainer.calculatedTremorIntensity >= 9.8f));

            // Paused -> Running if intensity < 8
            _fsm.AddTransition(_pausedState, _runningState, new FuncPredicate(() =>
                dataContainer.calculatedTremorIntensity < 8.0f));
            
            _fsm.SetState(_runningState);
        }

        private void Update() {
            _fsm.Update();
        }

        private void FixedUpdate() {
            _fsm.FixedUpdate();
        }
        
        /// <summary>
        /// Called by PausedState when entering the paused state.
        /// Sets the pause flag and starts the cooldown timer.
        /// </summary>
        public void HandlePauseEntered() {
            dataContainer.isPausedRecently = true;
            StartCoroutine(PauseCooldown());
        }

        /// <summary>
        /// Coroutine to reset the pause flag after 10 seconds.
        /// </summary>
        private IEnumerator PauseCooldown() {
            yield return new WaitForSeconds(10f);
            dataContainer.isPausedRecently = false;
            Debug.Log("Pause cooldown ended. isPausedRecently set to false.");
        }
    }
}