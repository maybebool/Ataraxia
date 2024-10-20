using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace SceneHandling {
    public class SceneLoader : Singleton<SceneLoader> {
        
        public UnityEvent OnLoadBegin = new();
        public UnityEvent OnLoadEnd = new();
        public ScreenFader screenFader;
        private bool _isLoading = false;

        private void Awake() {
            SceneManager.sceneLoaded += SetActiveScene;
        }

        private void OnDestroy() {
            SceneManager.sceneLoaded -= SetActiveScene;
        }

        public void LoadNewScene(string sceneName) {
            if (!_isLoading) {
                StartCoroutine(LoadScene(sceneName));
            }
        }

        private IEnumerator LoadScene(string sceneName) {
            _isLoading = true;
            OnLoadBegin?.Invoke();
            yield return screenFader.StartFadeIn();
            yield return StartCoroutine(UnloadCurrent());

            yield return new WaitForSeconds(3.0f);
            yield return StartCoroutine(LoadNew(sceneName));
            yield return screenFader.StartFadeOut();
            OnLoadEnd?.Invoke();
            _isLoading = false;
        }

        private IEnumerator UnloadCurrent() {
            var unloadOperation = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            while (!unloadOperation.isDone) {
                yield return null;
            }
        }

        private IEnumerator LoadNew(string sceneName) {
            var loadOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            while (!loadOperation.isDone) {
                yield return null;
            }
        }

        private void SetActiveScene(Scene scene, LoadSceneMode mode) {
            SceneManager.SetActiveScene(scene);
        }
    }
}