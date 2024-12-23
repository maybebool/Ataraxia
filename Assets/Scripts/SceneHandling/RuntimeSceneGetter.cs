using GameUI;
using UnityEngine;

namespace SceneHandling {
    public class RuntimeSceneGetter : MonoBehaviour {
        [RuntimeInitializeOnLoadMethod]
        static void OnRuntimeMethodLoad() {
            if (PlayerPrefs.HasKey("SceneToLoad")) {
                var sceneIndex = PlayerPrefs.GetInt("SceneToLoad");
                PlayerPrefs.DeleteKey("SceneToLoad");
                PlayerPrefs.Save();
                
                var sceneToLoad = (SceneNames)sceneIndex;
                SceneLoader.EnsureInstanceExists();
                SceneLoader.Instance.LoadNewScene(sceneToLoad);
            }
        }
    }
}