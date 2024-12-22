using Audio;
using Managers;
using ScriptableObjects;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

namespace Editor.Components.Buttons {
    
    [UxmlElement("QuitButton")]
    public partial class QuitButton : Button {
        
        private DataContainer dataContainer;
        public QuitButton() {
            var quitButtonStyle = Resources.Load<StyleSheet>("Styles/RestartButtonStyle");
            if (quitButtonStyle != null) {
                styleSheets.Add(quitButtonStyle);
                AddToClassList("customRestartButtonStyle");
            }
            else {
                Debug.LogError("Failed to load StyleSheet: restartButton.uss");
            }
            
            style.marginLeft = 0;
            style.marginRight = 0;
            var quitImage = Resources.Load<Texture2D>("Images/QuitButton");
            style.backgroundImage = quitImage;
            clicked += OnButtonClicked;
        }

        private void OnButtonClicked()
        {
            QuitApplication();
        }

        private void QuitApplication()
        {
#if UNITY_EDITOR
            
            EditorApplication.isPlaying = false;
            MtsEventManager.Instance.ClearDataContainer();
#else

            Application.Quit();
            MtsEventManager.Instance.ClearDataContainer();
#endif
        }
    }
}