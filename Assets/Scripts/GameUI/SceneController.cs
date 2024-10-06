using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameUI {
    public class SceneController : MonoBehaviour {
        [SerializeField] private Button startButton;


        private void OnEnable() {
            startButton.onClick.AddListener(OnClickStartButton);
        }
        private void OnDisable() {
            startButton.onClick.RemoveListener(OnClickStartButton);
        }


        private void OnClickStartButton() {
            SceneManager.LoadScene(1);
        }
    }
}