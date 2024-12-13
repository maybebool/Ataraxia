using System.Collections.Generic;
using GameUI;
using UnityEngine;
using UnityEngine.Audio;


namespace Audio {
    
    [System.Serializable]
    public class AudioClipGroup {
        public AudioMixerGroup mixerGroup;     
        public List<AudioClip> audioClips;    
    }
    
    [CreateAssetMenu(fileName = "Profiles", menuName = "Scriptable Objects/Profiles")]
    public class Profiles : ScriptableObject {

        public bool saveInPlayerPrefs = true;
        public string prefPrefix = "Settings_";
        public AudioMixer audioMixer;
        public Volume[] volumeControl;
        
        [Header("Assign Audio Clips to Mixer Groups")]
        public List<AudioClipGroup> audioClipGroups; 

        public void SetProfile(Profiles profile) {
            Settings.profile = profile;
        }

        public float GetAudioLevels(string name) {
            float volume = 1;
            if (!audioMixer) {
                Debug.LogWarning("There is no AudioMixer defined in the profiles file");
                return volume;
            }

            for (int i = 0; i < volumeControl.Length; i++) {
                if (volumeControl[i].name != name) {
                }
                else {
                    if (saveInPlayerPrefs) {
                        if (PlayerPrefs.HasKey(prefPrefix + volumeControl[i].name)) {
                            volumeControl[i].volume = PlayerPrefs.GetFloat(prefPrefix + volumeControl[i].name);
                        }
                    }

                    volumeControl[i].tempVolume = volumeControl[i].volume;
                    if (audioMixer) {
                        audioMixer.SetFloat(volumeControl[i].name, Mathf.Log(volumeControl[i].volume) * 20f);
                    }

                    volume = volumeControl[i].volume;
                    break;
                }
            }

            return volume;
        }

        public void GetAudioLevels() {
            if (!audioMixer) {
                Debug.LogWarning("There is no AudioMixer defined in the profiles file");
                return;
            }

            for (int i = 0; i < volumeControl.Length; i++) {
                if (saveInPlayerPrefs) {
                    if (PlayerPrefs.HasKey(prefPrefix + volumeControl[i].name)) {
                        volumeControl[i].volume = PlayerPrefs.GetFloat(prefPrefix + volumeControl[i].name);
                    }
                }

                volumeControl[i].tempVolume = volumeControl[i].volume;
                audioMixer.SetFloat(volumeControl[i].name, Mathf.Log(volumeControl[i].volume) * 20f);
            }
        }

        public void SetAudioLevels(string name, float volume) {
            if (!audioMixer) {
                Debug.LogWarning("There is no AudioMixer defined in the profiles file");
                return;
            }

            for (int i = 0; i < volumeControl.Length; i++) {
                if (volumeControl[i].name != name) {
                }
                else {
                    audioMixer.SetFloat(volumeControl[i].name, Mathf.Log(volume) * 20f);
                    volumeControl[i].tempVolume = volume;
                    break;
                }
            }
        }

        public void SaveAudioLevels() {
            if (!audioMixer) {
                Debug.LogWarning("There is no AudioMixer defined in the profiles file");
                return;
            }

            for (int i = 0; i < volumeControl.Length; i++) {
                var volume = volumeControl[i].tempVolume;
                if (saveInPlayerPrefs) {
                    PlayerPrefs.SetFloat(prefPrefix + volumeControl[i].name, volume);
                }
                audioMixer.SetFloat(volumeControl[i].name, Mathf.Log(volume) * 20f);
                volumeControl[i].volume = volume;
            }
        }

        public void SetAudioVolumeByToggle(string name, bool isEnabled) {
            if (!audioMixer) {
                Debug.LogWarning("AudioMixer ist in den Profileinstellungen nicht definiert.");
                return;
            }

            var volumeEntry = FindVolumeByName(name);
            if (volumeEntry == null) {
                Debug.LogWarning($"Kein Volume-Eintrag mit dem Namen '{name}' gefunden.");
                return;
            }

            var volume = isEnabled ? 1f : 0f;
            audioMixer.SetFloat(volumeEntry.name, volume);
            volumeEntry.tempVolume = volume;
        }

        private Volume FindVolumeByName(string levelName) {
            foreach (var volume in volumeControl) {
                if (volume.name == levelName) {
                    return volume;
                }
            }
            return null;
        }
    }
}


