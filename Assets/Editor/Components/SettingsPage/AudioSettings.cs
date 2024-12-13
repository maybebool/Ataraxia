using Audio;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Components.SettingsPage {
    public class AudioSettings : VisualElement {
        
        public AudioSettings() {
            
            var audioSettingsStyle = Resources.Load<StyleSheet>("Styles/AudioSettingsStyle");
            var audioSettingsUxml = Resources.Load<VisualTreeAsset>("SoundSettingsContainer");
            if (audioSettingsStyle != null) {
                styleSheets.Add(audioSettingsStyle);
                AddToClassList("custom-audio-settings-container");
            }
            if (audioSettingsUxml != null) {
                audioSettingsUxml.CloneTree(this);
                
            }else {
                Debug.LogError(
                    "Failed to load StyleSheet: BoxPlotStyle.uss.");
            }
            
            // style.opacity = Application.isPlaying ? 1f : 0.5f; 

            var audioToggle = this.Q<Toggle>("AudioToggle");
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
            
            var saveButton = this.Q<Button>("SaveButton");
            var resetButton = this.Q<Button>("DefaultSettingsButton");
            
            audioToggle.value = true;
            overallSlider.value = 1;
            musicSlider.value = 1;
            uISFXSlider.value = 1;
            gameSFXSlider.value = 1;
            
            audioToggle.RegisterValueChangedCallback(evt => { 
                OnAudioToggleChanged("Master",evt.newValue);
            });
            
            SliderEvent(musicSlider, musicValueLabel, "Music");
            SliderEvent(overallSlider, overallValueLabel, "Master");
            SliderEvent(uISFXSlider, interfaceSFXValueLabel, "UISFX");
            SliderEvent(gameSFXSlider, gameSFXValueLabel, "GameSFX");
            
            song1Btn?.RegisterCallback<ClickEvent>(_ => OnSongButtonClicked(0));
            song2Btn?.RegisterCallback<ClickEvent>(_ => OnSongButtonClicked(1));
            song3Btn?.RegisterCallback<ClickEvent>(_ => OnSongButtonClicked(2));
            song4Btn?.RegisterCallback<ClickEvent>(_ => OnSongButtonClicked(3));
            
            saveButton?.RegisterCallback<ClickEvent>(_ => OnSaveButtonClicked());
            resetButton?.RegisterCallback<ClickEvent>(_ => OnResetButtonClicked());
        }
        
        private void SliderEvent(Slider triggeredSlider, Label label, string audioMixerLevel) {
            triggeredSlider.RegisterValueChangedCallback(evt =>
                OnSliderValueChanged(label, audioMixerLevel, evt.newValue));
        }

        
        private void OnSliderValueChanged(Label volumeLabel, string audioMixerLevel, float value) {
            if (volumeLabel != null) {
                volumeLabel.text = Mathf.Round(value * 100f) + "%";
            }

            if (Settings.profile) {
                Settings.profile.SetAudioLevels(audioMixerLevel, value);
            }
        }

        private void OnAudioToggleChanged(string levelName, bool isOn) {
            if (Settings.profile != null) {
                Settings.profile.SetAudioVolumeByToggle(levelName, isOn);

                // Optionally, provide some feedback:
                Debug.Log("Audio toggled " + (isOn ? "on" : "off"));
            }
        }
        
        private void OnSongButtonClicked(int clipIndex) {
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

        private void OnSaveButtonClicked() {
            if (Settings.profile != null) {
                Settings.profile.SaveAudioLevels();
            }
        }

        private void OnResetButtonClicked() {
            AudioController.Instance.CancelChanges();
        }
    }
}
