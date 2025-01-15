using Managers;
using UnityEngine;

namespace Exercises.ExerciseOne {
    public class ObjectSpawnController : MonoBehaviour {
        
        [SerializeField] private ObjectPoolManager poolManager;
        [SerializeField] private float spawnInterval = 3f;
        [SerializeField] private float targetSpawnInterval = 5f;
        private float nextSpawnTime;
        private float _nextObstacleSpawnTime;
        private float _nextTargetSpawnTime;

        private void Update() {
            // Spawn main obstacles
            if (Time.time >= _nextObstacleSpawnTime) {
                poolManager.SpawnObstacleObjectFromPool(); // main obstacles
                _nextObstacleSpawnTime = Time.time + spawnInterval;
            }
            
            if (poolManager.spawnTargetObjects) {
                if (Time.time >= _nextTargetSpawnTime) {
                    poolManager.SpawnTargetObjectFromPool(); // second prefab
                    _nextTargetSpawnTime = Time.time + targetSpawnInterval;
                }
            }
        }
    }
}