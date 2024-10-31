using System;
using UnityEngine;

namespace Audio {
    [Serializable]
    public class Sound : MonoBehaviour {
        [SerializeField] public SoundNames soundName;
        [SerializeField] private AudioClip audioClip;
        [SerializeField] private bool audioLoop;
        [SerializeField] [Range(0, 1)] private float audioVolume;
        [SerializeField] [Range(0, 1)] private float audioSpatialBlend;
        [SerializeField] private float soundDistance;


        /// <summary>
        /// Constructor that holds all the parameters of the class
        /// </summary>
        /// <param name="soundName">Enum of possible sounds</param>
        /// <param name="audioClip">AudioClip itself</param>
        /// <param name="audioLoop"> boolean if sound should played in loop</param>
        /// <param name="audioVolume">float for volume</param>
        /// <param name="audioSpatialBlend">flaot for spatial blend</param>
        /// <param name="soundDistance"> how far the sound is audible</param>
        public Sound(SoundNames soundName, AudioClip audioClip, bool audioLoop, float audioVolume,
            float audioSpatialBlend, float soundDistance) {
            this.soundName = soundName;
            this.audioClip = audioClip;
            this.audioLoop = audioLoop;
            this.audioVolume = audioVolume;
            this.audioSpatialBlend = audioSpatialBlend;
            this.soundDistance = soundDistance;
        }

        /// <summary>
        /// Plays the audioClip of the given AudioSource
        /// </summary>
        /// <param name="audioSource">parameter of type AudioSource</param>
        public void AttachSound(AudioSource audioSource) {
            audioSource.clip = audioClip;
            audioSource.loop = audioLoop;
            audioSource.volume = audioVolume;
            audioSource.spatialBlend = audioSpatialBlend;
            audioSource.maxDistance = soundDistance;

            audioSource.Play();
        }

        /// <summary>
        /// Stops the audioClip of the given AudioSource
        /// </summary>
        /// <param name="audioSource">parameter of type AudioSource</param>
        public void StopSound(AudioSource audioSource) {
            audioSource.clip = audioClip;
            audioSource.loop = audioLoop;
            audioSource.volume = audioVolume;
            audioSource.spatialBlend = audioSpatialBlend;
            audioSource.maxDistance = soundDistance;

            audioSource.Pause();
        }
    }
}