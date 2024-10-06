using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameUI {
    public class SceneController : MonoBehaviour {
        [SerializeField] private Button scene1Button;
        [SerializeField] private Button scene0Button;


        private void OnEnable() {
            scene0Button.onClick.AddListener(OnCLickScene0StartButton);
            scene1Button.onClick.AddListener(OnCLickScene1StartButton);
        }
        private void OnDisable() {
            scene0Button.onClick.RemoveListener(OnCLickScene0StartButton);
            scene1Button.onClick.RemoveListener(OnCLickScene1StartButton);
        }


        private void OnCLickScene1StartButton() {
            SceneManager.LoadScene(1);
            Debug.Log("Clicked 1");
        }
        
        private void OnCLickScene0StartButton() {
            SceneManager.LoadScene(0);
            Debug.Log("Clicked 0");
        }
    }
}