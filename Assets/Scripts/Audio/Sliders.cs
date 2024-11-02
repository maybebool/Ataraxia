using System;
using UnityEngine;
using UnityEngine.UI;

namespace Audio {
    [RequireComponent(typeof(Slider))]
    public class Sliders : MonoBehaviour {
        
        Slider slider => GetComponent<Slider>();

        [Header("Volume Name")] 
        [SerializeField] private string volumeName = "enter here";
        
        [Header("Volume Label")] 
        [SerializeField] private Text volumeLabel;

        private void Start() {
            UpdateValueOnChange(slider.value);
            slider.onValueChanged.AddListener(delegate {
                UpdateValueOnChange(slider.value);
            });

        }

        public void UpdateValueOnChange(float value) {
            if (volumeLabel != null) {
                volumeLabel.text = Mathf.Round(value * 100f).ToString() + "%";
            }

            if (Settings.profile) {
                Settings.profile.SetAudioLevels(volumeName, value);
            }
        }

        public void ResetSliderValue() {
            if (Settings.profile) {
                var volume = Settings.profile.GetAudioLevels(volumeName);
                
                UpdateValueOnChange(volume);
                slider.value = volume;

            }
        }
    }
}