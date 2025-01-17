using Audio;
using Managers;
using UnityEngine;

namespace Exercises.ExerciseOne {
    public class ObjectCollisionHandler : MonoBehaviour {
        
        [Header("Audio Clip Settings")]
        [SerializeField] private int obstacleAudioClipIndex = 4;
        [SerializeField] private int targetAudioClipIndex = 5;
        [SerializeField] private int mixerIndex = 2; 
        
        [Header("Layer Indices")]
        [SerializeField] private int obstacleLayerIndex = 7; 
        [SerializeField] private int targetLayerIndex = 8;
        
        private void OnTriggerEnter(Collider other) {
            CheckCollision(other.gameObject);
            AudioController.Instance.PlayAudioClip(targetAudioClipIndex, mixerIndex);
        }

        private void CheckCollision(GameObject collidedObject) {
            var collidedLayer = collidedObject.layer;
            if (collidedLayer == obstacleLayerIndex) {
                AudioController.Instance.PlayAudioClip(obstacleAudioClipIndex, mixerIndex);
            }
            
            else if (collidedLayer == targetLayerIndex) {
                collidedObject.SetActive(false);
                AudioController.Instance.PlayAudioClip(targetAudioClipIndex, mixerIndex);
                MtsEventManager.Instance.onAllObstacleCourseTargetsCollectedUnityEvent?.Invoke();
            }
        }
    }
}
