using System;
using System.Collections;
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

            var sceneCount = SceneManager.sceneCount;
            foreach (var binding in bindings) {
                foreach (var obj in binding.exerciseInputGo) {
                        obj.SetActive(false);
                }
            }
            
            for (int i = 0; i < sceneCount; i++) {
                var s = SceneManager.GetSceneAt(i);
                
                if (!s.isLoaded) continue;
                var matchingBinding = Array.Find(bindings, b => b.buildIndex == s.buildIndex);
                
                if (matchingBinding == null) continue;
                foreach (var obj in matchingBinding.exerciseInputGo) {
                    obj.SetActive(true);
                }
            }
        }
    }
}