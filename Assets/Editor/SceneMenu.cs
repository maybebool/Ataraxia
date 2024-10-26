using UnityEditor;
using UnityEditor.SceneManagement;

namespace Editor {
    public static class SceneMenu
    {
        [MenuItem("Scenes/Menu")]
        public static void OpenMenu()
        {
            OpenScene("MainMenu");
        }

        public static void OpenGame()
        {
            OpenScene("TestScene");
        }

        private static void OpenScene(string sceneName)
        {
            EditorSceneManager.OpenScene("Assets/Scenes/Persistent.unity", OpenSceneMode.Single);
            EditorSceneManager.OpenScene("Assets/Scenes/" + sceneName + ".unity", OpenSceneMode.Additive);
        }
    }
}
