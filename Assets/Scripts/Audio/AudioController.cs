using System.Collections.Generic;
using GameUI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Audio {
    public class AudioController : MonoBehaviour {
        
        [Header("Audio Mixer Settings")]
        [SerializeField] private Profiles audioMixerProfileSo;
        [SerializeField] private int groupIndex = 0;
        private List<AudioSource> _audioSources = new();
        private Dictionary<int, List<AudioSource>> _audioSourcesByGroup = new();


        private void Awake() {
            if (audioMixerProfileSo != null) {
                audioMixerProfileSo.SetProfile(audioMixerProfileSo);
            }
        }

        private void Start() {
            if (Settings.profile && Settings.profile.audioMixer != null) {
                Settings.profile.GetAudioLevels();
                CheckAndProcessMainMenuAudio();
                BindClipsInMixerGroupToAudioSource(groupIndex);
                BackgroundMusic();
            }
        }

        void CheckAndProcessMainMenuAudio() {
            if (SceneManager.GetActiveScene().name == nameof(SceneNames.MainMenu)) {
                BindClipsInMixerGroupToAudioSource(1);
            }
        }

        private void BindClipsInMixerGroupToAudioSource(int index) {
            if (audioMixerProfileSo == null || audioMixerProfileSo.audioClipGroups == null) {
                Debug.LogError("AudioController is not assigned or has no audioClipGroups");
                return;
            }
            
            if (index < 0 || index >= audioMixerProfileSo.audioClipGroups.Count) {
                Debug.LogError("Index out of range");
                return;
            }
            
            var group = audioMixerProfileSo.audioClipGroups[index];
            var groupAudioSources = new List<AudioSource>();
            
            foreach (AudioClip clip in group.audioClips) {
                var source = gameObject.AddComponent<AudioSource>();
                source.clip = clip;
                source.outputAudioMixerGroup = group.mixerGroup;
                source.playOnAwake = false;
                source.loop = false;
                
                _audioSources.Add(source);
                groupAudioSources.Add(source);
            }
            
            _audioSourcesByGroup[index] = groupAudioSources;
        }

        private void BackgroundMusic() {
            if (audioMixerProfileSo == null) {
                Debug.LogError("AudioData (AudioClipMixerAttacher) is not assigned.");
                return;
            }

            if (audioMixerProfileSo.audioClipGroups.Count == 0) {
                Debug.LogError("No AudioClipGroups available.");
                return;
            }

            var group = audioMixerProfileSo.audioClipGroups[0];

            if (group.audioClips.Count == 0) {
                Debug.LogError("No AudioClips in the background music group.");
                return;
            }

            var bgmObject = new GameObject("BackgroundMusicController");
            var source = bgmObject.AddComponent<AudioSource>();

            source.clip = group.audioClips[0];
            source.outputAudioMixerGroup = group.mixerGroup;
            source.playOnAwake = true;
            source.loop = true;
            source.Play();

            bgmObject.AddComponent<DontDestroy>();
        }

        public void PlayAudioClip(int index) {
            if (index >= 0 && index < _audioSources.Count) {
                _audioSources[index].Play();
            }
            else {
                Debug.LogError("AudioSource index out of range.");
            }
        }

        public void PlayAudioClip(int index, int mixerIndex) {
            // Check if the mixerIndex exists in the dictionary
            if (!_audioSourcesByGroup.ContainsKey(mixerIndex)) {
                Debug.LogError(
                    $"Mixer group index {mixerIndex} not found. Make sure to process the mixer group before playing clips.");
                return;
            }

            var groupAudioSources = _audioSourcesByGroup[mixerIndex];

            // Check if index is within range
            if (index >= 0 && index < groupAudioSources.Count) {
                groupAudioSources[index].Play();
            }
            else {
                Debug.LogError($"Audio clip index {index} out of range in mixer group {mixerIndex}.");
            }
        }
    }
}