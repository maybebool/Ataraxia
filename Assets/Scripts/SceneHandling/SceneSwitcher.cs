using UnityEngine.SceneManagement;

namespace SceneHandling {
    public class SceneSwitcher : Singleton<SceneSwitcher> {
        
        private string sceneToLoad = null;
        

        private void Update() {
            // If we have a target scene to load and it's different from the current one
            if (string.IsNullOrEmpty(sceneToLoad)) return;
            var currentScene = SceneManager.GetActiveScene().name;
            if (currentScene == sceneToLoad) return;
            // Load the new scene at runtime, staying in play mode
            SceneManager.LoadScene(sceneToLoad);
            sceneToLoad = null; // Reset after loading
        }

        // Public method to be called from the Editor to change scene
        public void SetSceneToLoad(string sceneName) {
            sceneToLoad = sceneName;
        }
    }
}