using System.Collections;
using GameUI;
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

        public void LoadNewScene(SceneNames sceneNumber) {
            if (!_isLoading) {
                StartCoroutine(LoadScene(sceneNumber));
            }
        }

        private IEnumerator LoadScene(SceneNames sceneNumber) {
            _isLoading = true;
            OnLoadBegin?.Invoke();
            yield return screenFader.StartFadeIn();
            yield return StartCoroutine(UnloadCurrent());

            yield return new WaitForSeconds(3.0f);
            yield return StartCoroutine(LoadNew(sceneNumber));
            yield return screenFader.StartFadeOut();
            OnLoadEnd?.Invoke();
            _isLoading = false;
        }

        private IEnumerator UnloadCurrent() {
            var unloadOperation = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            while (unloadOperation != null && !unloadOperation.isDone) {
                yield return null;
            }
        }

        private IEnumerator LoadNew(SceneNames sceneNumber) {
            var loadOperation = SceneManager.LoadSceneAsync((int)sceneNumber, LoadSceneMode.Additive);
            while (loadOperation != null && !loadOperation.isDone) {
                yield return null;
            }
        }

        private void SetActiveScene(Scene scene, LoadSceneMode mode) {
            SceneManager.SetActiveScene(scene);
        }

        public static void EnsureInstanceExists() {
            if (Instance == null) {
                var gameObject = new GameObject("SceneLoader");
                gameObject.AddComponent<SceneLoader>();
                gameObject.AddComponent<DontDestroy>();
            }
        }
    }
}