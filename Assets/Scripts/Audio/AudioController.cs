using System.Collections.Generic;
using GameUI;
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

        public void BackgroundMusic() {
            // Check if the audioData is assigned
            if (audioMixerProfileSo == null) {
                Debug.LogError("AudioData (AudioClipMixerAttacher) is not assigned.");
                return;
            }

            // Check if there is at least one group
            if (audioMixerProfileSo.audioClipGroups.Count == 0) {
                Debug.LogError("No AudioClipGroups available.");
                return;
            }

            // Get the AudioClipGroup at index 0
            var group = audioMixerProfileSo.audioClipGroups[0];

            // Ensure there is at least one audio clip
            if (group.audioClips.Count == 0) {
                Debug.LogError("No AudioClips in the background music group.");
                return;
            }

            // Create a new GameObject for the background music
            var bgmObject = new GameObject("BackgroundMusic");

            // Add an AudioSource component
            var source = bgmObject.AddComponent<AudioSource>();

            // Assign the AudioClip to the AudioSource
            source.clip = group.audioClips[0]; // Use the first audio clip

            // Assign the AudioMixerGroup to the AudioSource
            source.outputAudioMixerGroup = group.mixerGroup;

            // Optional settings
            source.playOnAwake = true;
            source.loop = true;

            // Play the background music
            source.Play();

            // Add the DontDestroy script to the GameObject
            bgmObject.AddComponent<DontDestroy>();
        }
    }
}