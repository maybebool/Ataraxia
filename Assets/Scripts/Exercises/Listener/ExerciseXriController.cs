using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Exercises.Listener {

    [Serializable]
    public class BuildIndexGameObjectBinding {
        public int buildIndex;
        public GameObject[] exerciseInputGo;
    }

    public class ExerciseXriController : MonoBehaviour {
        [SerializeField] private BuildIndexGameObjectBinding[] bindings;
        private Coroutine _sceneCheckCoroutine;

        private void OnEnable() {
            _sceneCheckCoroutine = StartCoroutine(SceneCheckRoutine());
        }

        private void OnDisable() {
            if (_sceneCheckCoroutine != null) {
                StopCoroutine(_sceneCheckCoroutine);
            }
        }

        private IEnumerator SceneCheckRoutine() {
            while (true) {
                SceneCheckForInputObjectHandling();
                yield return new WaitForSeconds(2f);
            }
        }

        private void SceneCheckForInputObjectHandling() {
            var loadedIndexes = new HashSet<int>();
            var sceneCount = SceneManager.sceneCount;
            
            for (int i = 1; i < sceneCount; i++) {
                var s = SceneManager.GetSceneAt(i);
                if (s.isLoaded) {
                    loadedIndexes.Add(s.buildIndex);
                }
            }

            foreach (var binding in bindings) {
                var isLoaded = loadedIndexes.Contains(binding.buildIndex);
                foreach (var obj in binding.exerciseInputGo) {
                    obj.SetActive(isLoaded);
                }
            }
        }
    }
}