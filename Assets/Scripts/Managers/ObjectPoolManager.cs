using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Managers {
    public class ObjectPoolManager : MonoBehaviour {
        
        [Header("Main Prefab & Pool Settings")]
        [SerializeField] private GameObject obstaclePrefab;
        [SerializeField] private int obstaclePoolSize = 20; 
        [SerializeField] private int obstacleMaxPoolSize = 100;
        [SerializeField] private LayerMask collisionLayer; 
        [SerializeField] private float checkRadius = 1f; 

        [Header("Target Prefab & Pool Settings")] 
        [SerializeField] public bool spawnTargetObjects = false;
        [SerializeField] private GameObject targetPrefab;
        [SerializeField] private int targetPoolSize = 10;
        [SerializeField] private int targetMaxPoolSize = 50;

        [Header("Start Positions & Spawn Area")] 
        [SerializeField] private Vector3 spawnPosition;
        [SerializeField] private float xRange = 4f;
        [SerializeField] private float yRange = 2f;
        [SerializeField] private float xNoSpawn = 1.5f;
        [SerializeField] private float yNoSpawn = 3f;
        [SerializeField] private Vector3 endPosition;

        [Header("Movement")] 
        [SerializeField] private float moveSpeed = 1f;
        [SerializeField] private bool moveForward = true;

        private ObjectPool<GameObject> _obstacleObjectPool;
        private ObjectPool<GameObject> _targetObjectPool;
        private List<GameObject> _activeObstacles = new();
        private List<GameObject> _activeTargets = new();

        private float _nextOtherSpawnTime;

        private void Awake() {
            _obstacleObjectPool = new ObjectPool<GameObject>(
                createFunc: CreateObstacleObject,
                actionOnGet: OnGetObject,
                actionOnRelease: OnReleaseObject,
                actionOnDestroy: Destroy,
                collectionCheck: false,
                defaultCapacity: obstaclePoolSize,
                maxSize: obstacleMaxPoolSize
            );

            _targetObjectPool = new ObjectPool<GameObject>(
                createFunc: CreateTargetObject,
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
            
            if (spawnTargetObjects) {
                MoveActiveObjects(_activeTargets);
            }
        }

        public void SpawnObstacleObjectFromPool() {
            var obstacle = _obstacleObjectPool.Get();
            var xy = GetRandomXYWithNoSpawnArea(xRange, xNoSpawn, yRange, yNoSpawn);
            var pos = new Vector3(spawnPosition.x + xy.x, spawnPosition.y + xy.y, spawnPosition.z);
            
            obstacle.transform.position = pos;
            _activeObstacles.Add(obstacle);
        }

        public void SpawnTargetObjectFromPool() {
            if (!spawnTargetObjects) return;

            var targetObj = _targetObjectPool.Get();
            var xy = GetRandomXYWithNoSpawnArea(xRange, xNoSpawn, yRange, yNoSpawn);
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
                if (!obj) continue;

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

        private Vector2 GetRandomXYWithNoSpawnArea(float xRange, float xNoSpawn, float yRange, float yNoSpawn) {
            
            const int maxAttempts = 100;
            var bottom = -yRange;
            var bottomNoSpawnTop = bottom + yNoSpawn; 
    
            for (int i = 0; i < maxAttempts; i++) {
                var x = Random.Range(-xRange, xRange);
                var y = Random.Range(-yRange, yRange);
                var isXInNoSpawn = (Mathf.Abs(x) < xNoSpawn);
                var isYInBottomNoSpawn = (y >= bottom && y <= bottomNoSpawnTop);

                if (isXInNoSpawn && isYInBottomNoSpawn) {
                    continue;
                }
                
                var spawnPos = transform.position + new Vector3(x, y, 0f);
                var collidesWithLayer = Physics.CheckSphere(spawnPos, checkRadius, collisionLayer);
                if (collidesWithLayer) {
                    continue;
                }
                return new Vector2(x, y);
            }
            
            return new Vector2(Random.Range(-xRange, xRange), Random.Range(-yRange, yRange));
        }
        
        private void RePoolObjects(GameObject obj) {
            if (obj.name.Contains(obstaclePrefab.name)) {
                _obstacleObjectPool.Release(obj);
            } else {
                _targetObjectPool.Release(obj);
            }
        }
        
        private GameObject CreateObstacleObject() {
            var go = Instantiate(obstaclePrefab);
            go.name = obstaclePrefab.name + " (ObstaclePool)"; 
            return go;
        }

        private GameObject CreateTargetObject() {
            var go = Instantiate(targetPrefab);
            go.name = targetPrefab.name + " (TargetPool)";
            return go;
        }

        private void OnGetObject(GameObject obj) {
            obj.SetActive(true);
        }

        private void OnReleaseObject(GameObject obj) {
            obj.SetActive(false);
        }
    }
}