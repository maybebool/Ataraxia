using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Components.Buttons {
    [UxmlElement("PauseResumeToggle")]
    public partial class PauseResumeToggle : VisualElement {
        private Toggle _timeScaleToggle;

        public PauseResumeToggle() {
            
            var pauseResumeStyle = Resources.Load<StyleSheet>("Styles/PauseResumeStyle");
            if (pauseResumeStyle != null) {
                styleSheets.Add(pauseResumeStyle);
                AddToClassList("customPauseResumeStyle");
                style.height = new StyleLength(new Length(100, LengthUnit.Percent));
                style.justifyContent = Justify.FlexEnd;
                
                var uxml = Resources.Load<VisualTreeAsset>("PauseResumeToggle");
                uxml.CloneTree(this);
            }
            
            _timeScaleToggle = this.Q<Toggle>("TimeScaleToggle");

            if (_timeScaleToggle != null) {
                Time.timeScale = 1f;
                _timeScaleToggle.value = false;
                _timeScaleToggle.RegisterValueChangedCallback(evt => OnTimeScaleToggleChanged(evt.newValue));
            }
            else {
                Debug.LogWarning("TimeScaleToggle not found in UXML.");
            }
        }
        
        private void OnTimeScaleToggleChanged(bool isOn) {
            Time.timeScale = isOn ? 0f : 1f;
            Debug.Log("TimeScaleToggle changed to: " + isOn);
            var toggleInput = _timeScaleToggle.Q<VisualElement>(className: "unity-toggle__input");
            if (toggleInput != null) {
                // Load or reference the textures you want to use for different states
                // These could be loaded from Resources, assigned via code, or set up as fields on the class.
                var newBackgroundTexture = isOn 
                    ? Resources.Load<Texture2D>("Images/TogglePressed") 
                    : Resources.Load<Texture2D>("Images/ToggleUnpressed");
        
                // Update the background image style
                if (newBackgroundTexture != null) {
                    toggleInput.style.backgroundImage = new StyleBackground(newBackgroundTexture);
                }
                else {
                    Debug.LogWarning("Failed to load background image for toggle state.");
                }
            }
            else {
                Debug.LogWarning("Could not find element with class 'unity-toggle__input'.");
            }
        }
    }
}