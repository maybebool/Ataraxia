using GameUI;
using SceneHandling;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Components.Buttons {
    [UxmlElement("StartButton")]
    public partial class StartButton : Button {
        private SceneNames sceneToStart;

        public StartButton() {
        }

        public StartButton(SceneNames scene) {
            sceneToStart = scene;
            Initialize();
            RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
            RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);
        }

        private void Initialize() {

            var startButton = Resources.Load<StyleSheet>("Styles/StartButtonStyle");
            if (startButton != null) {
                styleSheets.Add(startButton);
                AddToClassList("custom-start-button");
            } else {
                Debug.LogError("Failed to load StyleSheet: StartButtonStyle.uss");
            }

            var startButtonImage = Resources.Load<Texture2D>("Images/PlayButton");
            style.backgroundImage = startButtonImage;
        }

        private void OnAttachToPanel(AttachToPanelEvent e) {
            clicked -= OnButtonClicked;
            clicked += OnButtonClicked;
        }

        private void OnDetachFromPanel(DetachFromPanelEvent e) {
            clicked -= OnButtonClicked;
        }

        private void OnButtonClicked() {
            if (!Application.isPlaying) {
                // Not in play mode: Store sceneToLoad and start play mode
                PlayerPrefs.SetInt("SceneToLoad", (int)sceneToStart);
                PlayerPrefs.Save();

                EditorApplication.delayCall += () => { EditorApplication.isPlaying = true; };
            } else {
                // In play mode: attempt to change scenes at runtime if needed
                if (!IsSceneActive(sceneToStart)) {
                    TryChangeSceneAtRuntimeUsingSceneLoader();
                }
            }
        }

        private bool IsSceneActive(SceneNames scene) {
            var activeSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            return activeSceneName == scene.ToString();
        }

        public void TryChangeSceneAtRuntimeUsingSceneLoader() {
            if (!Application.isPlaying) {
                Debug.LogWarning("Not in play mode, cannot change scene at runtime.");
                return;
            }

            // If we are here, we assume scene is different; call SceneLoader to load the scene async
            SceneLoader.EnsureInstanceExists();
            if (SceneLoader.Instance == null) {
                Debug.LogError("SceneLoader instance not found.");
                return;
            }

            SceneLoader.Instance.LoadNewScene(sceneToStart);
            Debug.Log($"Requested async load of {sceneToStart} at runtime.");
        }
    }
}