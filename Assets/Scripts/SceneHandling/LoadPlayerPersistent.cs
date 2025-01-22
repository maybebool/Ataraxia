using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneHandling {
    public class LoadPlayerPersistent : MonoBehaviour {
        
        private void Awake() {
            if (!Application.isEditor)
                LoadPersistent();
        }

        private void LoadPersistent() {
            SceneManager.LoadSceneAsync("Scenes/MainMenu", LoadSceneMode.Additive);
        }
    }
}