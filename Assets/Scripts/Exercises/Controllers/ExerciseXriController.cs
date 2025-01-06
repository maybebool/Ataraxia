using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Exercises.Controllers {
    [Serializable]
    public class BuildIndexGameObjectBinding {
        public int buildIndex;
        public GameObject[] exerciseInputGo;
    }

    public class ExerciseXriController : MonoBehaviour {
        [SerializeField] private BuildIndexGameObjectBinding[] bindings;
        private Coroutine _sceneCheckCoroutine;
        private int lastSceneHash = 0;

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
                CheckForSceneChange();
                yield return new WaitForSeconds(2f);
            }
        }

        private void CheckForSceneChange() {
            var currentSceneHash = GetCurrentScenesHash();

            if (currentSceneHash == lastSceneHash) return;
            lastSceneHash = currentSceneHash;
            HandleBindingsForScenes();
        }

        private int GetCurrentScenesHash() {
            var hash = 17;
            for (int i = 1; i < SceneManager.sceneCount; i++) {
                var scene = SceneManager.GetSceneAt(i);
                if (scene.isLoaded) {
                    hash = hash * 31 + scene.buildIndex;
                }
            }
            return hash;
        }


        private void HandleBindingsForScenes() {
            DeactivateAllBindings();

            var sceneCount = SceneManager.sceneCount;

            for (int i = 1; i < sceneCount; i++) {
                var s = SceneManager.GetSceneAt(i);

                if (!s.isLoaded) continue;
                var matchingBinding = Array.Find(bindings, b => b.buildIndex == s.buildIndex);

                if (matchingBinding == null) continue;
                foreach (var obj in matchingBinding.exerciseInputGo) {
                    obj.SetActive(true);
                }
            }
        }

        private void DeactivateAllBindings() {
            foreach (var binding in bindings) {
                foreach (var obj in binding.exerciseInputGo) {
                    obj.SetActive(false);
                }
            }
        }
    }
}