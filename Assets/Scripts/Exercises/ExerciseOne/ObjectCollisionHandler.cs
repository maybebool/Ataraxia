using Audio;
using UnityEngine;

namespace Exercises.ExerciseOne {
    public class ObjectCollisionHandler : MonoBehaviour {
        
        [Header("Audio Clip Settings")]
        [SerializeField] private int obstacleAudioClipIndex = 1;
        [SerializeField] private int obstacleMixerIndex = 1; 
        [SerializeField] private int targetAudioClipIndex = 1;
        [SerializeField] private int targetMixerIndex = 1;
        
        [Header("Layer Indices")]
        [SerializeField] private int obstacleLayerIndex = 7; 
        [SerializeField] private int targetLayerIndex = 8;
        
        private void OnTriggerEnter(Collider other) {
            CheckCollision(other.gameObject);
        }

        private void CheckCollision(GameObject collidedObject) {
            var collidedLayer = collidedObject.layer;
            if (collidedLayer == obstacleLayerIndex) {
                AudioController.Instance.PlayAudioClip(obstacleAudioClipIndex, obstacleMixerIndex);
            }
            
            else if (collidedLayer == targetLayerIndex) {
                collidedObject.SetActive(false);
                AudioController.Instance.PlayAudioClip(targetAudioClipIndex, targetMixerIndex);
            }
        }
    }
}
