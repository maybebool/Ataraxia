using Audio;
using SceneHandling;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace GameUI {
    public class UIController : MonoBehaviour {
    
        [Header("Main Menu Home Buttons")]
        [SerializeField] private Button mainButtonHome;
        [SerializeField] private Button startExercisesButton;
        
        [Header("Main Menu Settings Buttons")]
        [SerializeField] private Button audioToggleOnOff;
        [SerializeField] private Button song1Button;
        [SerializeField] private Button song2Button;
        [SerializeField] private Button song3Button;
        [SerializeField] private Button song4Button;
        [SerializeField] private Button saveChangesButton;
        [SerializeField] private Button defaultAudioSettingsButton;

        private int _indexSong1 = 0;
        private AudioController _audioControl;
    

        private void OnEnable() {
            UIUtil.CallMultipleActions(startExercisesButton, OnClickStartExercisesButton, ()=> OnUIButtonClick(1));
            UIUtil.CallMultipleActions(song1Button,() =>OnClickSongButton(0), ()=>OnUIButtonClick(1));
        
        }

        private void OnDisable() {
            startExercisesButton.onClick.RemoveAllListeners();
            song1Button.onClick.RemoveAllListeners();
            
        
        }

        private void OnClickStartExercisesButton() {
            SceneLoader.Instance.LoadNewScene((int)SceneNames.Scene1);
        }

        private void OnClickBackToMainMenuButton() {
            SceneLoader.Instance.LoadNewScene((int)SceneNames.MainMenu);
        }

        private void OnClickSongButton(int songIndex) {
            _audioControl.PlayAudioClip(songIndex, 0);
        }
        
        private void OnUIButtonClick(int audioClipIndex) {
            _audioControl.PlayAudioClip(audioClipIndex,1);
        }
        

    
    }
}
