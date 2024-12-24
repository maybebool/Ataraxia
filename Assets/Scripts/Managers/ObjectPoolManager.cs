using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Managers {
    public class ObjectPoolManager : MonoBehaviour {
        [Header("Prefab & Pool Settings")] 
        [SerializeField] private GameObject cubePrefab;

        [SerializeField] private int poolSize = 20; // Default capacity
        [SerializeField] private int maxPoolSize = 100; // Max capacity

        [Header("Positions")]
        [Tooltip("Base position where cubes spawn. We will add random +/- x,y offsets.")]
        [SerializeField] private Vector3 spawnPosition;

        [SerializeField] private float xRange = 5f;
        [SerializeField] private float yRange = 2f;

        [Tooltip("Once a cube's z position surpasses this endPosition.z, we re-pool it.")] 
        [SerializeField] private Vector3 endPosition;

        [Header("Movement")] 
        [SerializeField] private float moveSpeed = 1f;

        [Tooltip("If true, we'll move cubes so they increase in z from spawnPos.z -> endPos.z. "
                 + "If false, we do the reverse check, e.g. if endPosition.z < spawnPosition.z, etc.")]
        [SerializeField] private bool moveForward = true;

        private ObjectPool<GameObject> _cubePool;
        private List<GameObject> _activeCubes = new();

        private void Awake() {
            _cubePool = new ObjectPool<GameObject>(
                createFunc: CreateCube,
                actionOnGet: OnGetCube,
                actionOnRelease: OnReleaseCube,
                actionOnDestroy: Destroy,
                collectionCheck: false,
                defaultCapacity: poolSize,
                maxSize: maxPoolSize
            );
        }

        private void Update() {
            MoveActiveCubes();
        }

        /// <summary>
        /// Spawn a single cube from the pool at a random x,y around spawnPosition.
        /// </summary>
        public void SpawnCubeFromPool() {
            // "Get" a cube from the pool
            var cube = _cubePool.Get();

            // Random offset for x,y
            var randX = Random.Range(-xRange, xRange);
            var randY = Random.Range(-yRange, yRange);

            var randomSpawnPos = new Vector3(
                spawnPosition.x + randX,
                spawnPosition.y + randY,
                spawnPosition.z
            );
            cube.transform.position = randomSpawnPos;

            // Keep track of active cubes
            _activeCubes.Add(cube);
        }

        /// <summary>
        /// Moves each active cube along the z-axis (or you could do a vector direction).
        /// Once a cube crosses endPosition.z, we return it to the pool and remove from _activeCubes.
        /// </summary>
        private void MoveActiveCubes() {
            // We'll store which cubes we want to re-pool in a temporary list
            List<GameObject> cubesToRelease = null;

            for (int i = 0; i < _activeCubes.Count; i++) {
                var cube = _activeCubes[i];
                if (cube == null) continue; // safety check

                // Move the cube. For a simple z-based approach:
                var step = moveSpeed * Time.deltaTime;
                // If you're only changing z:
                var pos = cube.transform.position;
                if (moveForward) {
                    pos.z += step;
                }
                else {
                    pos.z -= step;
                }

                cube.transform.position = pos;

                // Check if we have passed endPosition.z
                if (moveForward) {
                    if (cube.transform.position.z >= endPosition.z) {
                        if (cubesToRelease == null) cubesToRelease = new List<GameObject>();
                        cubesToRelease.Add(cube);
                    }
                }
                else {
                    if (cube.transform.position.z <= endPosition.z) {
                        if (cubesToRelease == null) cubesToRelease = new List<GameObject>();
                        cubesToRelease.Add(cube);
                    }
                }
            }

            // Now re-pool them
            if (cubesToRelease != null) {
                foreach (var cube in cubesToRelease) {
                    RePoolCube(cube);
                }
            }
        }

        /// <summary>
        /// Re-pool the cube via the ObjectPool. 
        /// This calls 'Release' on the pool, which triggers OnReleaseCube.
        /// </summary>
        private void RePoolCube(GameObject cube) {
            // Remove from _activeCubes
            _activeCubes.Remove(cube);
            // Return to the pool
            _cubePool.Release(cube);
        }

        // == ObjectPool Delegates == //

        private GameObject CreateCube() {
            // Called if the pool needs to create a new object
            var obj = Instantiate(cubePrefab);
            return obj;
        }

        private void OnGetCube(GameObject cube) {
            // Called whenever we .Get() from the pool
            cube.SetActive(true);
        }

        private void OnReleaseCube(GameObject cube) {
            // Called whenever we .Release() back to the pool
            cube.SetActive(false);
        }
    }
}