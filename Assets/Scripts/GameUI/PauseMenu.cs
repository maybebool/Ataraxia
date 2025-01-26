using Audio;
using SceneHandling;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace GameUI {
    public class PauseMenu : MonoBehaviour {
         [SerializeField] private GameObject pauseMenu;
         [SerializeField] private Button continueButton;
         [SerializeField] private Button quitButton;

         private void OnEnable() {
             UIUtil.CallMultipleActions(continueButton, OnClickContinueButton, ()=>OnClickUIButton(0));
             UIUtil.CallMultipleActions(quitButton, OnClickQuitButton, ()=>OnClickUIButton(0));
         }

         private void OnDisable() {
             continueButton.onClick.RemoveAllListeners();
             quitButton.onClick.RemoveAllListeners();
         }
         
         private void OnClickUIButton(int audioClipIndex) {
             AudioController.Instance.PlayAudioClip(audioClipIndex,1);
         }

        private void OnClickContinueButton() {
            Time.timeScale = 1;
             pauseMenu.SetActive(false);
         }
        
        private void OnClickQuitButton() {
            pauseMenu.SetActive(false);
            SceneLoader.Instance.LoadNewScene(SceneNames.MainMenu);
        }
    }
}