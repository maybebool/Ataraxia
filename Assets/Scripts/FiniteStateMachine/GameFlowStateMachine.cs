﻿using FiniteStateMachine.States;
using ScriptableObjects;
using UnityEngine;

namespace FiniteStateMachine {
    public class GameFlowStateMachine : MonoBehaviour {
        [SerializeField] private DataContainer dataContainer;
        [SerializeField] private GameObject pauseMenu; 
        [SerializeField] private string mainMenuSceneName = "MainMenu";

        private StateMachine _fsm;
        private RunningState _runningState;
        private PausedState _pausedState;
        private CancelState _cancelState;

        private void Awake() {
            _fsm = new StateMachine();
        
            _runningState = new RunningState();
            _pausedState = new PausedState(pauseMenu);
            _cancelState = new CancelState(mainMenuSceneName);

            
            // Running -> Paused if intensity in [8, 9.8)
            _fsm.AddTransition(_runningState, _pausedState, new FuncPredicate(() =>
                dataContainer.calculatedTremorIntensity >= 8.0f &&
                dataContainer.calculatedTremorIntensity < 9.8f));

            // Running -> Cancel if intensity >= 9.8
            _fsm.AddTransition(_runningState, _cancelState, new FuncPredicate(() =>
                dataContainer.calculatedTremorIntensity >= 9.8f));

            // Paused -> Running if intensity < 8
            _fsm.AddTransition(_pausedState, _runningState, new FuncPredicate(() =>
                dataContainer.calculatedTremorIntensity < 8.0f));

            // Paused -> Cancel if intensity >= 9.8
            _fsm.AddTransition(_pausedState, _cancelState, new FuncPredicate(() =>
                dataContainer.calculatedTremorIntensity >= 9.8f));
            
            _fsm.SetState(_runningState);
        }

        private void Update() {
            _fsm.Update();
        }

        private void FixedUpdate() {
            _fsm.FixedUpdate();
        }
    }
}