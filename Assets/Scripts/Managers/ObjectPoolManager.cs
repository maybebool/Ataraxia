using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;

namespace Managers {
    public class ObjectPoolManager : MonoBehaviour {
        
        [FormerlySerializedAs("cubePrefab")]
        [Header("Prefab & Pool Settings")] 
        [SerializeField] private GameObject objectPrefab;
        [SerializeField] private int poolSize = 20; // Default capacity
        [SerializeField] private int maxPoolSize = 100; // Max capacity

        [Header("Positions")]
        [SerializeField] private Vector3 spawnPosition;
        [SerializeField] private float xRange = 5f;
        [SerializeField] private float yRange = 2f;
        [SerializeField] private Vector3 endPosition;

        [Header("Movement")] 
        [SerializeField] private float moveSpeed = 1f;
        [SerializeField] private bool moveForward = true;

        private ObjectPool<GameObject> _objectPool;
        private List<GameObject> _activeObjects = new();

        private void Awake() {
            _objectPool = new ObjectPool<GameObject>(
                createFunc: CreateObject,
                actionOnGet: OnGetObject,
                actionOnRelease: OnReleaseObject,
                actionOnDestroy: Destroy,
                collectionCheck: false,
                defaultCapacity: poolSize,
                maxSize: maxPoolSize
            );
        }

        private void Update() {
            MoveActiveObjects();
        }
        
        public void SpawnObjectFromPool() {
            var objToSpawn = _objectPool.Get();
            var randX = Random.Range(-xRange, xRange);
            var randY = Random.Range(-yRange, yRange);

            var randomSpawnPos = new Vector3(
                spawnPosition.x + randX,
                spawnPosition.y + randY,
                spawnPosition.z
            );
            objToSpawn.transform.position = randomSpawnPos;
            _activeObjects.Add(objToSpawn);
        }
        
        private void MoveActiveObjects() {
            List<GameObject> objectsToRelease = null;

            foreach (var obj in _activeObjects) {
                if (!obj) continue;
                
                var step = moveSpeed * Time.deltaTime;
                var pos = obj.transform.position;
                if (moveForward) {
                    pos.z += step;
                }
                else {
                    pos.z -= step;
                }

                obj.transform.position = pos;
                
                if (moveForward) {
                    if (!(obj.transform.position.z >= endPosition.z)) continue;
                }
                else {
                    if (!(obj.transform.position.z <= endPosition.z)) continue;
                }

                objectsToRelease ??= new List<GameObject>();
                objectsToRelease.Add(obj);
            }

            if (objectsToRelease == null) return; {
                foreach (var obj in objectsToRelease) {
                    RePoolObjects(obj);
                }
            }
        }
        
        private void RePoolObjects(GameObject obj) {
            _activeObjects.Remove(obj);
            _objectPool.Release(obj);
        }

        //ObjectPool Delegates
        
        private GameObject CreateObject() {
            var obj = Instantiate(objectPrefab);
            return obj;
        }

        private void OnGetObject(GameObject obj) {
            obj.SetActive(true);
        }

        private void OnReleaseObject(GameObject obj) {
            obj.SetActive(false);
        }
    }
}