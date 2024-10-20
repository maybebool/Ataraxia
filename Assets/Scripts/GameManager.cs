using System;
using SceneHandling;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    [SerializeField] private Button startFirstLevelButton;
    [SerializeField] private Button backToMenuButton;
    [SerializeField] private Button nextButton;

    private void OnEnable() {
        startFirstLevelButton.onClick.AddListener(OnClickStartButton);
        backToMenuButton.onClick.AddListener(OnClickMenuButton);
        nextButton.onClick.AddListener(OnClickNextButton);
    }

    private void OnDisable() {
        startFirstLevelButton.onClick.RemoveListener(OnClickStartButton);
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
