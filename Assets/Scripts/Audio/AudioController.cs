using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Audio {
    public class AudioController : MonoBehaviour {
        [SerializeField] private Profiles m_profiles;
        [SerializeField] private List<Sliders> volumeSliders = new();


        
        private void Awake() {
            if (m_profiles != null) {
                m_profiles.SetProfile(m_profiles);
            }
        }

        private void Start() {
            if (Settings.profile && Settings.profile.audioMixer != null) {
                Settings.profile.GetAudioLevels();
            }
        }

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