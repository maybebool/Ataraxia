using StateMachines;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CancelState : BaseState {
    private string mainMenuScene;

    public CancelState(string mainMenuSceneName) {
        mainMenuScene = mainMenuSceneName;
    }

    public override void OnEnter() {
        // Option 1: Immediately load the main menu
        Debug.Log("Entering Cancel (Quit) State -> Loading Main Menu");
        SceneManager.LoadScene(mainMenuScene);
        
        // Option 2: If you want a fade-out or some UI message before quitting,
        // you can add that logic here instead.
    }
}