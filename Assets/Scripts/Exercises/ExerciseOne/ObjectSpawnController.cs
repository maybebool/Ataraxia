using Managers;
using UnityEngine;

namespace Exercises.ExerciseOne {
    public class ObjectSpawnController : MonoBehaviour {
        
        [SerializeField] private ObjectPoolManager poolManager;
        [SerializeField] private float spawnInterval = 1f;
        private float nextSpawnTime;

        private void Update() {
            if (!(Time.time >= nextSpawnTime)) return;
            poolManager.SpawnObjectFromPool();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }
}