using Audio;
using Managers;
using UnityEngine;

namespace Exercises.ExerciseTwo {
    public abstract class MazeTargetCollisionBase : MonoBehaviour {
        [SerializeField] protected int obstacleAudioClipIndex = 4;
        [SerializeField] protected int targetAudioClipIndex = 5;
        [SerializeField] protected int mixerIndex = 2; 
        protected abstract string targetTag { get; }
        private void OnTriggerEnter(Collider other) {
            CheckCollision(other.gameObject);
        }

        private void CheckCollision(GameObject collidedObject) {
            
            if (!collidedObject.CompareTag(targetTag)) {
                AudioController.Instance.PlayAudioClip(obstacleAudioClipIndex, mixerIndex);
            }
            
            else {
                collidedObject.SetActive(false);
                AudioController.Instance.PlayAudioClip(targetAudioClipIndex, mixerIndex);
                MtsEventManager.Instance.onAllMazeTargetsCollectedUnityEvent?.Invoke();
                gameObject.SetActive(false);
            }
        }
    }
}