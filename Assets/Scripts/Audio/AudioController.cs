using System.Collections.Generic;
using UnityEngine;

namespace Audio {
    public class AudioController : MonoBehaviour {
        [SerializeField] private Profiles audioMixerProfileSo;
        [SerializeField] private int groupIndex = 0; 
        private List<AudioSource> _audioSources = new();


        private void Awake() {
            if (audioMixerProfileSo != null) {
                audioMixerProfileSo.SetProfile(audioMixerProfileSo);
            }
        }

        private void Start() {
            if (Settings.profile && Settings.profile.audioMixer != null) {
                Settings.profile.GetAudioLevels();
                BindClipsInMixerGroupToAudioSource(groupIndex);
            }
        }

        public void BindClipsInMixerGroupToAudioSource(int index) {
            if (audioMixerProfileSo == null || audioMixerProfileSo.audioClipGroups == null) {
                Debug.LogError("AudioController is not assigned or has no audioClipGroups");
                return;
            }

            // Check if the index is within the valid range
            if (index < 0 || index >= audioMixerProfileSo.audioClipGroups.Count) {
                Debug.LogError("Index out of range");
                return;
            }

            // Get the AudioClipGroup at the specified index
            var group = audioMixerProfileSo.audioClipGroups[index];

            // Iterate through each AudioClip in the selected group
            foreach (AudioClip clip in group.audioClips) {
                var source = gameObject.AddComponent<AudioSource>();
                source.clip = clip;
                source.outputAudioMixerGroup = group.mixerGroup;
                source.playOnAwake = false;
                source.loop = false;
                _audioSources.Add(source);
            }
        }
    }
}