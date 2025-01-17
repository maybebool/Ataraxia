using Audio;
using Managers;
using UnityEngine;

namespace Exercises.ExerciseTwo {
    public abstract class MazeTargetCollisionBase : MonoBehaviour {
        [SerializeField] private int obstacleAudioClipIndex = 4;
        [SerializeField] private int targetAudioClipIndex = 5;
        [SerializeField] private int mixerIndex = 2; 
        
        protected abstract int targetLayerIndex { get; set; }
        
        private void OnTriggerEnter(Collider other) {
            Debug.Log("Collision");
            CheckCollision(other.gameObject);
        }

        private void CheckCollision(GameObject collidedObject) {
            
            var collidedLayer = collidedObject.layer;
            if (collidedLayer != targetLayerIndex) {
                AudioController.Instance.PlayAudioClip(obstacleAudioClipIndex, mixerIndex);
            }

            else if (collidedLayer == targetLayerIndex) {
                collidedObject.SetActive(false);
                AudioController.Instance.PlayAudioClip(targetAudioClipIndex, mixerIndex);
                MtsEventManager.Instance.onAllMazeTargetsCollectedUnityEvent?.Invoke();
                gameObject.SetActive(false);
            }
        }
    }
}