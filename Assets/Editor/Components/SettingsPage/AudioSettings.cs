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
                
            }else {
                Debug.LogError(
                    "Failed to load StyleSheet: BoxPlotStyle.uss. Make sure it's placed in a Resources/Styles/ folder.");
            }
            
            // TODO Button style on hover not working
            style.opacity = Application.isPlaying ? 1f : 0.5f; 


            var overallSlider = this.Q<Slider>("OverallSlider");
            var musicSlider = this.Q<Slider>("MusicSlider");
            var uISFXSlider = this.Q<Slider>("UISFXSlider");
            var gameSFXSlider = this.Q<Slider>("GameSFXSlider");
            
            var musicValueLabel = this.Q<Label>("MusicValue");
            var overallValueLabel = this.Q<Label>("OverallValue");
            var interfaceSFXValueLabel = this.Q<Label>("UISFXValue");
            var gameSFXValueLabel = this.Q<Label>("GameSFXValue");
            
            var song1Btn = this.Q<Button>("Song1Button");
            var song2Btn = this.Q<Button>("Song2Button");
            var song3Btn = this.Q<Button>("Song3Button");
            var song4Btn = this.Q<Button>("Song4Button");
            
            overallSlider.value = 100;
            musicSlider.value = 100;
            uISFXSlider.value = 100;
            gameSFXSlider.value = 100;
            
            SliderEvent(musicSlider, musicValueLabel, "Music");
            SliderEvent(overallSlider, overallValueLabel, "Overall");
            SliderEvent(uISFXSlider, interfaceSFXValueLabel, "UISFX");
            SliderEvent(gameSFXSlider, gameSFXValueLabel, "GameSFX");
            
            song1Btn?.RegisterCallback<ClickEvent>(_ => OnAudioButtonClicked(0));
            song2Btn?.RegisterCallback<ClickEvent>(_ => OnAudioButtonClicked(1));
            song3Btn?.RegisterCallback<ClickEvent>(_ => OnAudioButtonClicked(2));
            song4Btn?.RegisterCallback<ClickEvent>(_ => OnAudioButtonClicked(3));
        }
        
        private void SliderEvent(Slider triggeredSlider, Label label, string audioMixerLevel) {
            triggeredSlider.RegisterValueChangedCallback(evt => OnSliderValueChanged(label, audioMixerLevel, evt.newValue));
        }

        
        private void OnSliderValueChanged(Label volumeLabel, string audioMixerLevel, float value) {
            if (volumeLabel != null) {
                volumeLabel.text = Mathf.Round(value * 100f) + "%";
            }

            if (Settings.profile) {
                Settings.profile.SetAudioLevels(audioMixerLevel, value);
            }
        }
        
        private void OnAudioButtonClicked(int clipIndex) {
            if (!Application.isPlaying) {
                Debug.LogWarning("Not in play mode. AudioController is not accessible.");
                return;
            }
            
            if (AudioController.Instance == null) {
                Debug.LogError("AudioController not found in the scene.");
                return;
            }
            
            AudioController.Instance.BackgroundMusic(clipIndex);
        }
    }
}
