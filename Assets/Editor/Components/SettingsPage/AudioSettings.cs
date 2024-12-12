using Audio;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Components.SettingsPage {
    public class AudioSettings : VisualElement {
        
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
            
            var overallSlider = this.Q<Slider>("OverallSlider");
            var musicSlider = this.Q<Slider>("MusicSlider");
            var uISFXSlider = this.Q<Slider>("UISFXSlider");
            var gameSFXSlider = this.Q<Slider>("GameSFXSlider");
            
            var musicValueLabel = this.Q<Label>("MusicValue");
            var overallValueLabel = this.Q<Label>("OverallValue");
            var interfaceSFXValueLabel = this.Q<Label>("UISFXValue");
            var gameSFXValueLabel = this.Q<Label>("GameSFXValue");
            
            musicSlider.value = 100;
            
            SliderEvent(musicSlider, musicValueLabel, "Music");
            SliderEvent(overallSlider, overallValueLabel, "Overall");
            SliderEvent(uISFXSlider, interfaceSFXValueLabel, "UISFX");
            SliderEvent(gameSFXSlider, gameSFXValueLabel, "GameSFX");
        }
        
        private void SliderEvent(Slider triggeredSlider, Label label, string audioMixerLevel) {
            triggeredSlider.RegisterValueChangedCallback(evt => OnSliderValueChanged(label, audioMixerLevel, evt.newValue));
        }

        
        private void OnSliderValueChanged(Label volumeLabel, string audioMixerLevel, float value) {
            if (volumeLabel != null) {
                volumeLabel.text = Mathf.Round(value) + "%";
            }

            if (Settings.profile) {
                Settings.profile.SetAudioLevels(audioMixerLevel, value);
            }
        }
    }
}
