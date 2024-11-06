using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Audio {
    public class SoundHandler : MonoBehaviour {
        
        public static SoundHandler Current { get; private set; }
        public List<AudioSource> audioSources = new();
        public Sound[] sounds;
        [SerializeField] private int maxSounds;
        


        public void Awake() {
            if (Current != null && Current != this)
                Destroy(gameObject);
            else {
                Current = this;
            }
        }
        
        private AudioSource GetAudioSource() {
            foreach (var audioSource in audioSources.Where(audioSource => !audioSource.isPlaying)) {
                return audioSource;
            }

            if (audioSources.Count >= maxSounds)
                return null;

            var newAudioSource = new GameObject("Sound");
            newAudioSource.transform.SetParent(transform);
            audioSources.Add(newAudioSource.AddComponent<AudioSource>());
            return audioSources[^1];
        }


        /// <summary>
        /// iterates trough Array and checks for name match, returns the audioSource
        /// </summary>
        /// <param name="soundName">Enums for possible sounds</param>
        /// <param name="soundLocation">transform.position needed where the sound will play</param>
        /// <returns></returns>
        public AudioSource PlaySound(SoundNames soundName, Vector3 soundLocation) {
            var audioSource = GetAudioSource();

            if (audioSource == null)
                return null;

            foreach (var sound in sounds) {
                if (soundName != sound.soundName)
                    continue;
                audioSource.transform.position = soundLocation;
                sound.AttachSound(audioSource);
                return audioSource;
            }

            Debug.LogWarning($"Could not find sound {soundName}");
            return null;
        }

        public void StopSound(SoundNames soundName, Vector3 soundLocation) {
            var audioSource = GetAudioSource();

            if (audioSource == null)
                return;

            foreach (var sound in sounds) {
                if (soundName != sound.soundName)
                    continue;
                audioSource.transform.position = soundLocation;
                sound.StopSound(audioSource);
                Debug.Log("Stop sound function fired");
                return;
            }

            Debug.LogWarning($"Could not find sound {soundName}");
        }
    }
}