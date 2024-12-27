using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;

namespace Managers {
    public class ObjectPoolManager : MonoBehaviour {
        
        [Header("Main Prefab & Pool Settings")]
        [SerializeField] private GameObject obstaclePrefab;

        [SerializeField] private int obstaclePoolSize = 20; // Default capacity
        [SerializeField] private int obstacleMaxPoolSize = 100; // Max capacity

        [Header("Price Prefab")] [SerializeField]
        private bool spawnTargetObjects = false;
        [SerializeField] private GameObject targetPrefab;
        [SerializeField] private int targetPoolSize = 10;
        [SerializeField] private int targetMaxPoolSize = 50;
        [SerializeField] private float targetSpawnInterval = 5f; // how often we spawn "other" object

        [Header("Positions")] [SerializeField] private Vector3 spawnPosition;
        [SerializeField] private float xRange = 5f;
        [SerializeField] private float xNoSpawn = 2f;
        [SerializeField] private float yRange = 2f;
        [SerializeField] private float yNoSpawn = 1f;
        [SerializeField] private Vector3 endPosition;

        [Header("Movement")] [SerializeField] private float moveSpeed = 1f;
        [SerializeField] private bool moveForward = true;

        private ObjectPool<GameObject> _obstacleObjectPool;
        private ObjectPool<GameObject> _targetObjectPool;
        private List<GameObject> _activeObstacles = new();
        private List<GameObject> _activeTargets = new();

        private float _nextOtherSpawnTime;

        private void Awake() {
            _obstacleObjectPool = new ObjectPool<GameObject>(
                createFunc: CreateObject,
                actionOnGet: OnGetObject,
                actionOnRelease: OnReleaseObject,
                actionOnDestroy: Destroy,
                collectionCheck: false,
                defaultCapacity: obstaclePoolSize,
                maxSize: obstacleMaxPoolSize
            );

            _targetObjectPool = new ObjectPool<GameObject>(
                createFunc: CreateOtherObject,
                actionOnGet: OnGetObject,
                actionOnRelease: OnReleaseObject,
                actionOnDestroy: Destroy,
                collectionCheck: false,
                defaultCapacity: targetPoolSize,
                maxSize: targetMaxPoolSize
            );
        }

        private void Update() {
            MoveActiveObjects(_activeObstacles);

            // Move second prefab objects if used
            if (spawnTargetObjects) {
                MoveActiveObjects(_activeTargets);

                // Check if it's time to spawn another "other" object
                if (!(Time.time >= _nextOtherSpawnTime)) return;
                SpawnTargetObjectFromPool();
                _nextOtherSpawnTime = Time.time + targetSpawnInterval;
            }
        }

        public void SpawnObjectFromPool() {
            var obstacle = _obstacleObjectPool.Get();
            var xy = GetRandomXYWithNoSpawn(xRange, xNoSpawn, yRange, yNoSpawn);
            var pos = new Vector3(spawnPosition.x + xy.x, spawnPosition.y + xy.y, spawnPosition.z);
            
            obstacle.transform.position = pos;
            _activeObstacles.Add(obstacle);
        }

        public void SpawnTargetObjectFromPool() {
            if (!spawnTargetObjects) return;

            var targetObj = _targetObjectPool.Get();
            var xy = GetRandomXYWithNoSpawn(xRange, xNoSpawn, yRange, yNoSpawn);
            var pos = new Vector3(spawnPosition.x + xy.x, spawnPosition.y + xy.y, spawnPosition.z);
            
            targetObj.transform.position = pos;
            _activeTargets.Add(targetObj);
        }

        private void MoveActiveObjects(List<GameObject> objectsList) {
            if (objectsList.Count == 0) return;
            var step = moveSpeed * Time.deltaTime;

            List<GameObject> toRelease = null;

            for (int i = 0; i < objectsList.Count; i++) {
                var obj = objectsList[i];
                if (obj == null) continue;

                // Move z
                var pos = obj.transform.position;
                if (moveForward)
                    pos.z += step;
                else
                    pos.z -= step;
                obj.transform.position = pos;

                // Check if we've passed endPosition.z
                var shouldRelease = false;
                if (moveForward && pos.z >= endPosition.z) {
                    shouldRelease = true;
                }
                else if (!moveForward && pos.z <= endPosition.z) {
                    shouldRelease = true;
                }

                if (shouldRelease) {
                    if (toRelease == null) toRelease = new List<GameObject>();
                    toRelease.Add(obj);
                }
            }

            if (toRelease != null) {
                for (int i = 0; i < toRelease.Count; i++) {
                    var obj = toRelease[i];
                    objectsList.Remove(obj);
                    RePoolObjects(obj);
                }
            }
        }

        private Vector2 GetRandomXYWithNoSpawn(float xRange, float xNoSpawn, float yRange, float yNoSpawn)
        {
            const int maxAttempts = 100;
            for (int i = 0; i < maxAttempts; i++)
            {
                float x = Random.Range(-xRange, xRange);
                float y = Random.Range(-yRange, yRange);

                // Skip if BOTH x and y are within no-spawn 
                // (i.e. |x| < xNoSpawn && |y| < yNoSpawn)
                if (Mathf.Abs(x) < xNoSpawn && Mathf.Abs(y) < yNoSpawn)
                {
                    // This means we are in the small "no-spawn" rectangle
                    // so we try again
                    continue;
                }

                // Otherwise, it's valid
                return new Vector2(x, y);
            }

            // Fallback if we never found a valid one
            return new Vector2(Random.Range(-xRange, xRange),
                Random.Range(-yRange, yRange));
        }

        //ObjectPool Delegates
        
        private void RePoolObjects(GameObject obj) {
            if (obj.name.Contains(obstaclePrefab.name)) {
                _obstacleObjectPool.Release(obj);
            } else {
                _targetObjectPool.Release(obj);
            }
        }


        private GameObject CreateObject() {
            var go = Instantiate(obstaclePrefab);
            go.name = obstaclePrefab.name + " (Pool)"; // optional rename
            return go;
        }

        private GameObject CreateOtherObject() {
            var go = Instantiate(targetPrefab);
            go.name = targetPrefab.name + " (OtherPool)";
            return go;
        }

        private void OnGetObject(GameObject obj) {
            obj.SetActive(true);
        }

        private void OnReleaseObject(GameObject obj) {
            obj.SetActive(false);
        }
        
        private void OnDrawGizmos() {
            Gizmos.color = Color.blue;
            var totalSize = new Vector3(xRange * 2f, yRange * 2f, 0.1f);
            var totalCenter = spawnPosition; 
            Gizmos.DrawWireCube(totalCenter, totalSize);
            Gizmos.color = Color.red;
            var noSpawnSize = new Vector3(xNoSpawn * 2f, yNoSpawn * 2f, 0.1f);
            var noSpawnCenter = spawnPosition;
            Gizmos.DrawWireCube(noSpawnCenter, noSpawnSize);
        }
    }
}