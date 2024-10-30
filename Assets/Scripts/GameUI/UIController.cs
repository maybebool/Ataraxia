using System;
using SceneHandling;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
    [SerializeField] private Button startExercisesButton;
    [SerializeField] private Button backToMenuButton;
    [SerializeField] private Button nextButton;

    private void OnEnable() {
        startExercisesButton.onClick.AddListener(OnClickStartButton);
        backToMenuButton.onClick.AddListener(OnClickMenuButton);
        nextButton.onClick.AddListener(OnClickNextButton);
    }

    private void OnDisable() {
        startExercisesButton.onClick.RemoveListener(OnClickStartButton);
        backToMenuButton.onClick.RemoveListener(OnClickMenuButton);
        nextButton.onClick.RemoveListener(OnClickNextButton);
    }

    private void OnClickStartButton() {
        SceneLoader.Instance.LoadNewScene("TestScene");
    }

    private void OnClickMenuButton() {
        SceneLoader.Instance.LoadNewScene("MainMenu");
    }

    private void OnClickNextButton() {
        SceneLoader.Instance.LoadNewScene("NextScene");
    }
}
