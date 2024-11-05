using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Audio {
    [RequireComponent(typeof(Slider))]
    public class Sliders : MonoBehaviour {
        
        Slider slider => GetComponent<Slider>();
        [SerializeField] private string volumeName = "enter name here";
        [SerializeField] private TMP_Text volumeLabel;

        private void Start() {
            ResetSliderValue();
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
                var volume = 1;
                
                UpdateValueOnChange(volume);
                slider.value = volume;

            }
        }
    }
}