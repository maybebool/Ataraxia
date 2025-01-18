using Audio;
using UnityEngine;

namespace Exercises.ExerciseTwo {
    public class MazeCollision : MonoBehaviour{
        
        [SerializeField] protected int obstacleAudioClipIndex = 4;
        [SerializeField] protected int mixerIndex = 2;
        private const string TargetTag = "Maze";

        private void OnTriggerEnter(Collider other) {
            CheckCollision(other.gameObject);
        }

        private void CheckCollision(GameObject collidedObject) {
            if (collidedObject.CompareTag(TargetTag)) {
                AudioController.Instance.PlayAudioClip(obstacleAudioClipIndex, mixerIndex);
            }
        }
    }
}