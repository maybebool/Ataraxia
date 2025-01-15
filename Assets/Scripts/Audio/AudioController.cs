using System.Collections.Generic;
using GameUI;
using Managers;
using UnityEngine;

namespace Audio {
    public class AudioController : Singleton<AudioController> {
        [Header("Audio Mixer Settings")]
        [SerializeField] private Profiles audioMixerProfileSo;
        [SerializeField] private int groupIndex = 0;
        [SerializeField] private List<Sliders> volumeSliders = new();
        
        private List<AudioSource> _audioSources = new();
        private Dictionary<int, List<AudioSource>> _audioSourcesByGroup = new();


        private void Awake() {
            if (audioMixerProfileSo != null) {
                audioMixerProfileSo.SetProfile(audioMixerProfileSo);
            }
        }
        
        private void OnEnable() {
            if (MtsEventManager.Instance != null) {
                MtsEventManager.Instance.OnMainMenuLoaded += HandleMainMenuAudio;
                MtsEventManager.Instance.OnExerciseLoaded += HandleExerciseAudio;
            }
        }

        private void OnDisable() {
            if (MtsEventManager.Instance != null) {
                MtsEventManager.Instance.OnMainMenuLoaded -= HandleMainMenuAudio;
                MtsEventManager.Instance.OnExerciseLoaded -= HandleExerciseAudio;
            }
        }

        private void Start() {
            if (Settings.profile && Settings.profile.audioMixer != null) {
                Settings.profile.GetAudioLevels();
                BackgroundMusic(0);
            }
        }
        
        private void HandleMainMenuAudio() {
            BindClipsInMixerGroupToAudioSource(1);
        }

        private void HandleExerciseAudio() {
            BindClipsInMixerGroupToAudioSource(2);
        }

        private void BindClipsInMixerGroupToAudioSource(int index) {
            if (_audioSourcesByGroup.TryGetValue(index, out var oldSources)) {
                foreach (var audioSource in oldSources) {
                    if (audioSource != null) {
                        Destroy(audioSource);
                    }
                }
                _audioSourcesByGroup[index].Clear();
            }
            
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

        public void BackgroundMusic(int clipIndex) {
            if (audioMixerProfileSo == null) {
                Debug.LogError("AudioData (AudioClipMixerAttacher) is not assigned.");
                return;
            }
            
            if (audioMixerProfileSo.audioClipGroups.Count == 0) {
                Debug.LogError("No AudioClipGroups available.");
                return;
            }

            var group = audioMixerProfileSo.audioClipGroups[0];
            
            if (clipIndex < 0 || clipIndex >= group.audioClips.Count) {
                Debug.LogError("Clip index out of range in background music group.");
                return;
            }

            var selectedClip = group.audioClips[clipIndex];
            var bgmObject = GameObject.Find("BackgroundMusic");

            if (bgmObject != null) {
                var source = bgmObject.GetComponent<AudioSource>();
                if (source != null) {
                    source.clip = selectedClip;
                    source.Play();
                }
                else {
                    Debug.LogError("BackgroundMusic GameObject does not have an AudioSource component.");
                }
            }
            else {
                
                bgmObject = new GameObject("BackgroundMusic");
                var source = bgmObject.AddComponent<AudioSource>();
                source.clip = selectedClip;
                source.outputAudioMixerGroup = group.mixerGroup;
                source.playOnAwake = true;
                source.loop = true;
                source.Play();
                bgmObject.AddComponent<DontDestroy>();
            }
            
            _audioSourcesByGroup[0] = new List<AudioSource> { bgmObject.GetComponent<AudioSource>() };
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
            if (!_audioSourcesByGroup.TryGetValue(mixerIndex, out var groupAudioSources)) {
                Debug.LogError(
                    $"Mixer group index {mixerIndex} not found. Make sure to process the mixer group before playing clips.");
                return;
            }

            if (index >= 0 && index < groupAudioSources.Count) {
                groupAudioSources[index].Play();
            }
            else {
                Debug.LogError($"Audio clip index {index} out of range in mixer group {mixerIndex}.");
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