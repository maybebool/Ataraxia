using GameUI;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Components.Buttons {
    [UxmlElement("SwitchButton")]
    public partial class SwitchButton : Button {
        private Texture2D startImage;
        private Texture2D pauseImage;
        private Texture2D playImage;
        private SceneNames sceneToStart;
        private string prefsKeyPlaying;
        private string prefsKeyPaused;
        private static bool isWaitingToRestart = false;
        
        public SwitchButton() { }
        
        public SwitchButton(SceneNames scene) {
            sceneToStart = scene;
            Initialize();
            RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
            RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);
            
        }
        
        private void Initialize() {
            prefsKeyPlaying = $"SwitchButton_{sceneToStart}_IsPlaying";
            prefsKeyPaused = $"SwitchButton_{sceneToStart}_IsPaused";
            var asset = Resources.Load<VisualTreeAsset>("SwitchButton");
            asset.CloneTree(this);

            var switchButton = Resources.Load<StyleSheet>("Styles/SwitchButtonStyle");
            if (switchButton != null) {
                styleSheets.Add(switchButton);
                AddToClassList("customSwitchButtonStyle");
            }
            else {
                Debug.LogError("Failed to load StyleSheet: SwitchButton.uss");
            }

            startImage = Resources.Load<Texture2D>("Images/PlayButton");
            pauseImage = Resources.Load<Texture2D>("Images/StopButton");
            playImage = Resources.Load<Texture2D>("Images/PlayButton");

            if (startImage == null || pauseImage == null || playImage == null) {
                Debug.LogError("Failed to load one or more images for the StartPauseButton.");
            }
            UpdateButtonImage();
        }
        
        private void OnAttachToPanel(AttachToPanelEvent e)
        {
            clicked -= OnButtonClicked;
            clicked += OnButtonClicked;
            // Subscribe to play mode and pause state changes
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            EditorApplication.pauseStateChanged -= OnPauseStateChanged;
            EditorApplication.pauseStateChanged += OnPauseStateChanged;

            // Update the button image based on the stored state
            UpdateButtonImage();
        }

        private void OnDetachFromPanel(DetachFromPanelEvent e)
        {
            clicked -= OnButtonClicked;
            // Unsubscribe from events to prevent memory leaks
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.pauseStateChanged -= OnPauseStateChanged;
        }

        private void OnButtonClicked() {
            if (!EditorApplication.isPlaying)
            {
                // Store the scene to load using PlayerPrefs
                PlayerPrefs.SetInt("SceneToLoad", (int)sceneToStart);
                PlayerPrefs.Save();

                // Set the playing state in EditorPrefs
                EditorPrefs.SetBool(prefsKeyPlaying, true);
                EditorPrefs.SetBool(prefsKeyPaused, false);

                // Start play mode
                EditorApplication.delayCall += () =>
                {
                    EditorApplication.isPlaying = true;
                };
            }
            else
            {
                // Check if this button's scene is active
                var isCurrentScene = IsSceneActive(sceneToStart);

                if (isCurrentScene)
                {
                    var isPaused = EditorPrefs.GetBool(prefsKeyPaused, false);

                    if (!isPaused)
                    {
                        // Pause play mode
                        EditorApplication.isPaused = true;
                        EditorPrefs.SetBool(prefsKeyPaused, true);
                    }
                    else
                    {
                        // Resume play mode
                        EditorApplication.isPaused = false;
                        EditorPrefs.SetBool(prefsKeyPaused, false);
                    }
                }
                else
                {
                    // Another scene is playing; prompt to stop and start new scene
                    if (EditorUtility.DisplayDialog("Scene Change",
                        "Another scene is currently playing. Do you want to stop it and start this scene?",
                        "Yes", "No"))
                    {
                        // Set flag to indicate we're waiting to restart play mode
                        isWaitingToRestart = true;

                        // Store the scene to load
                        PlayerPrefs.SetInt("SceneToLoad", (int)sceneToStart);
                        PlayerPrefs.Save();

                        // Stop play mode
                        EditorApplication.isPlaying = false;

                        // Set the playing state in EditorPrefs
                        EditorPrefs.SetBool(prefsKeyPlaying, true);
                        EditorPrefs.SetBool(prefsKeyPaused, false);
                    }
                }
            }

            // Update the button image
            UpdateButtonImage();
        }

        private void UpdateButtonImage() {
            var isPlaying = EditorPrefs.GetBool(prefsKeyPlaying, false);
            var isPaused = EditorPrefs.GetBool(prefsKeyPaused, false);

            if (!isPlaying) {
                // Show start image
                style.backgroundImage = new StyleBackground(startImage);
            }
            else if (isPaused) {
                // Show play image (for resume)
                style.backgroundImage = new StyleBackground(playImage);
            }
            else {
                // Show pause image
                style.backgroundImage = new StyleBackground(pauseImage);
            }
        }

        private void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredEditMode)
            {
                // Play mode has exited
                // Check if we need to restart play mode with a new scene
                if (isWaitingToRestart)
                {
                    isWaitingToRestart = false;
            
                    // Delay call to start play mode again
                    EditorApplication.delayCall += () =>
                    {
                        EditorApplication.isPlaying = true;
                    };
                }
                else
                {
                    // Reset playing state when exiting play mode
                    // ugly solution because every button is changing images this way, but at least it changes, original values where false, false 
                    EditorPrefs.SetBool(prefsKeyPlaying, false);
                    EditorPrefs.SetBool(prefsKeyPaused, false);
                }
            }
            UpdateButtonImage();
        }

        private void OnPauseStateChanged(PauseState state) {
            var isPaused = state == PauseState.Paused;
            EditorPrefs.SetBool(prefsKeyPaused, isPaused);
            UpdateButtonImage();
            Debug.Log("Pause is pressed");
            
        }
        
        private bool IsSceneActive(SceneNames scene) {
            // Compare the active scene name with the scene associated with this button
            var activeSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            return activeSceneName == scene.ToString();
            
        }
    }
}