using ScriptableObjects;
using StateMachines;
using StateMachines.States;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlowStateMachine : MonoBehaviour {
    [SerializeField] private DataContainer dataContainer;
    [SerializeField] private GameObject pauseMenu; 
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    private StateMachine fsm;
    
    // Our three states:
    private RunningState runningState;
    private PausedState pausedState;
    private CancelState cancelState;

    private void Awake() {
        fsm = new StateMachine();
        
        runningState = new RunningState();
        pausedState = new PausedState(pauseMenu);
        cancelState = new CancelState(mainMenuSceneName);

        // ===== Transitions =====
        // 1) Running -> Paused if intensity in [8, 9.8)
        fsm.AddTransition(runningState, pausedState, new FuncPredicate(() =>
            dataContainer.calculatedTremorIntensity >= 8.0f &&
            dataContainer.calculatedTremorIntensity < 9.8f));

        // 2) Running -> Cancel if intensity >= 9.8
        fsm.AddTransition(runningState, cancelState, new FuncPredicate(() =>
            dataContainer.calculatedTremorIntensity >= 9.8f));

        // 3) Paused -> Running if intensity < 8
        fsm.AddTransition(pausedState, runningState, new FuncPredicate(() =>
            dataContainer.calculatedTremorIntensity < 8.0f));

        // 4) Paused -> Cancel if intensity >= 9.8
        fsm.AddTransition(pausedState, cancelState, new FuncPredicate(() =>
            dataContainer.calculatedTremorIntensity >= 9.8f));

        // If you want to automatically leave CancelState after reloading main menu,
        // you can handle that differently (see note in CancelState below).

        // Set the initial state
        fsm.SetState(runningState);
    }

    private void Update() {
        fsm.Update();
    }

    private void FixedUpdate() {
        fsm.FixedUpdate();
    }
}