using System.Collections.Generic;
using UnityEngine;

namespace Audio {
    public class SliderHandling : MonoBehaviour {
        [SerializeField] private List<Sliders> volumeSliders = new();
        
        
        public void ApplyChanges() {
            if (Settings.profile && Settings.profile.audioMixer != null) {
                Settings.profile.SaveAudioLevels();
            }
        }

        public void CancelChanges() {
            if (Settings.profile && Settings.profile.audioMixer != null) {
                Settings.profile.GetAudioLevels();
            }

            for (int i = 0; i < volumeSliders.Count; i++) {
                volumeSliders[i].ResetSliderValue();
            }
        }
    }
}