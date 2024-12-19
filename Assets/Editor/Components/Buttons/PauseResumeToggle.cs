using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Components.Buttons {
    public class PauseResumeToggle : VisualElement {
        private Toggle _timeScaleToggle;

        public PauseResumeToggle() {
            
            var pauseResumeStyle = Resources.Load<StyleSheet>("Styles/PauseResumeStyle");
            if (pauseResumeStyle != null) {
                styleSheets.Add(pauseResumeStyle);
                AddToClassList("customPauseResumeStyle");
                
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
        }
    }
}