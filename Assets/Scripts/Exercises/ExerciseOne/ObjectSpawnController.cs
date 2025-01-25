using Managers;
using UnityEngine;

namespace Exercises.ExerciseOne {
    public class ObjectSpawnController : MonoBehaviour {
        
        [SerializeField] private ObjectPoolManager poolManager;
        [SerializeField] private float spawnInterval = 3f;
        [SerializeField] private float targetSpawnInterval = 5f;
        private float _nextSpawnTime;
        private float _nextObstacleSpawnTime;
        private float _nextTargetSpawnTime;

        private void Update() {
            if (Time.time >= _nextObstacleSpawnTime) {
                poolManager.SpawnObstacleObjectFromPool(); 
                _nextObstacleSpawnTime = Time.time + spawnInterval;
            }
            
            if (poolManager.spawnTargetObjects) {
                if (Time.time >= _nextTargetSpawnTime) {
                    poolManager.SpawnTargetObjectFromPool();
                    _nextTargetSpawnTime = Time.time + targetSpawnInterval;
                }
            }
        }
    }
}