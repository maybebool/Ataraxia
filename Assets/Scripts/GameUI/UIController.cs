using System;
using GameUI;
using SceneHandling;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
    [SerializeField] private Button startExercisesButton;
    [SerializeField] private Button backToMenuButton;
    [SerializeField] private Button pauseMenuButton;
    [SerializeField] private Button AudioButton;
    

    private void OnEnable() {
        startExercisesButton.onClick.AddListener(OnClickStartExercisesButton);
        backToMenuButton.onClick.AddListener(OnClickBackToMainMenuButton);
        
    }

    private void OnDisable() {
        startExercisesButton.onClick.RemoveListener(OnClickStartExercisesButton);
        backToMenuButton.onClick.RemoveListener(OnClickBackToMainMenuButton);
        
    }

    private void OnClickStartExercisesButton() {
        SceneLoader.Instance.LoadNewScene((int)SceneNames.Scene1);
    }

    private void OnClickBackToMainMenuButton() {
        SceneLoader.Instance.LoadNewScene((int)SceneNames.MainMenu);
    }

    
}
