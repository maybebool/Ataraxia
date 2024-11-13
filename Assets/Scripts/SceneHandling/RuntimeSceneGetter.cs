using GameUI;
using UnityEngine;

namespace SceneHandling {
    
    public class RuntimeSceneGetter : MonoBehaviour {
        
        [RuntimeInitializeOnLoadMethod]
        static void OnRuntimeMethodLoad()
        {
            if (PlayerPrefs.HasKey("SceneToLoad"))
            {
                var sceneIndex = PlayerPrefs.GetInt("SceneToLoad");
                var sceneToLoad = (SceneNames)sceneIndex;

                // Clear the PlayerPrefs key
                PlayerPrefs.DeleteKey("SceneToLoad");
                PlayerPrefs.Save();

                // Ensure SceneLoader instance exists
                SceneLoader.EnsureInstanceExists();

                // Now load the scene using SceneLoader
                SceneLoader.Instance.LoadNewScene(sceneToLoad);
            }
        }
    }
}