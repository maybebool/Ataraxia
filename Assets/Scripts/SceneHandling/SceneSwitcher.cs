using UnityEngine.SceneManagement;

namespace SceneHandling {
    public class SceneSwitcher : Singleton<SceneSwitcher> {
        
        private string _sceneToLoad;
        
        private void Update() {
            
            if (string.IsNullOrEmpty(_sceneToLoad)) return;
            var currentScene = SceneManager.GetActiveScene().name;
            if (currentScene == _sceneToLoad) return;
            
            SceneManager.LoadScene(_sceneToLoad);
            _sceneToLoad = null; 
        }
        
        public void SetSceneToLoad(string sceneName) {
            _sceneToLoad = sceneName;
        }
    }
}