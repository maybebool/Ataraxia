using Audio;
using Editor.Helpers;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Components.SettingsPage {
    public class AudioSettings : VisualElement {
        private string volumeName = "Music";
        private float volume;
        private Slider slider;
        
        
        
        public AudioSettings() {

            
            var audioSettingsStyle = Resources.Load<StyleSheet>("Styles/AudioSettingsStyle");
            var audioSettingsUxml = Resources.Load<VisualTreeAsset>("SoundSettingsContainer");
            if (audioSettingsStyle != null) {
                styleSheets.Add(audioSettingsStyle);
                AddToClassList("custom-audio-settings-container");
                AddToClassList("unity-button-custom");
            }
            if (audioSettingsUxml != null) {
                audioSettingsUxml.CloneTree(this);
            }
            else {
                Debug.LogError(
                    "Failed to load StyleSheet: BoxPlotStyle.uss. Make sure it's placed in a Resources/Styles/ folder.");
            }
            
            var mySlider = this.Q<Slider>("MusicSlider");
            var volumeLabel = this.Q<Label>("MusicValue");
            // slider = mySlider;
            mySlider.value = 100;
            
            // TODO this doesnt work. Somehow .Q doesnt get the name i guess

            mySlider.RegisterValueChangedCallback(evt => {
                var newValue = evt.newValue;
                OnSliderValueChanged(volumeLabel, newValue);
                    
            });

        }
        private void OnSliderValueChanged(Label volumeLabel, float value) {
            // Settings.profile.GetAudioLevels();
            Debug.Log("Slider reached via name");
            if (volumeLabel != null) {
                volumeLabel.text = Mathf.Round(value * 100f) + "%";
            }

            if (Settings.profile) {
                Settings.profile.SetAudioLevels(volumeName, value);
            }
        }
        
        public void ResetSliderValue(Label volumeLabel) {
            if (Settings.profile) {
                var volume = 1;
                
                OnSliderValueChanged(volumeLabel, volume);
                slider.value = volume;
            }
        }
    
    }
}
