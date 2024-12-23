using System;
using GameUI;
using SceneHandling;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Components.Buttons {
    [UxmlElement("RestartButton")]
    public partial class RestartButton : Button {
        public RestartButton() {

            var restartButton = Resources.Load<StyleSheet>("Styles/RestartButtonStyle");
            if (restartButton != null) {
                styleSheets.Add(restartButton);
                AddToClassList("customRestartButtonStyle");
            }
            else {
                Debug.LogError("Failed to load StyleSheet: restartButton.uss");
            }
            var restartImage = Resources.Load<Texture2D>("Images/RestartButton");
            style.backgroundImage = restartImage;
            
            clicked -= OnButtonClicked;
            clicked += OnButtonClicked;
        }

        private void OnButtonClicked() {
            var currentSceneEnum = GetCurrentSceneNameEnum();
            if (currentSceneEnum.HasValue) {
                Debug.Log($"Restarting scene: {currentSceneEnum.Value}");
                SceneLoader.Instance.LoadNewScene(currentSceneEnum.Value);
            }
            else {
                Debug.LogError("Failed to determine the current scene or scene is not managed by SceneLoader.");
            }
        }

        private SceneNames? GetCurrentSceneNameEnum() {
            var activeSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            foreach (SceneNames sceneName in Enum.GetValues(typeof(SceneNames))) {
                if (activeSceneName.Equals(sceneName.ToString(), StringComparison.OrdinalIgnoreCase)) {
                    return sceneName;
                }
            }
            return null;
        }
    }
}