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
        [SerializeField] private Toggle audioToggleOnOff;
        [SerializeField] private Button song1Button;
        [SerializeField] private Button song2Button;
        [SerializeField] private Button song3Button;
        [SerializeField] private Button song4Button;
        [SerializeField] private Button saveChangesButton;
        [SerializeField] private Button defaultAudioSettingsButton;
        
        public AudioController _audioControl;
        
        private void Awake()
        {
            if (_audioControl == null)
            {
                _audioControl = FindObjectOfType<AudioController>();
                if (_audioControl == null)
                {
                    Debug.LogError("AudioController not found in the scene.");
                }
            }
        }
        
        private void OnEnable() {
            UIUtil.CallMultipleActions(startExercisesButton, OnClickStartExercisesButton, ()=>OnUIButtonClick(2));
            UIUtil.CallMultipleActions(song1Button,()=>OnClickSongButton(0), ()=>OnUIButtonClick(2));
            UIUtil.CallMultipleActions(song2Button,()=>OnClickSongButton(1), ()=>OnUIButtonClick(2));
            UIUtil.CallMultipleActions(song3Button,()=>OnClickSongButton(2), ()=>OnUIButtonClick(2));
            UIUtil.CallMultipleActions(song4Button,()=>OnClickSongButton(0), ()=>OnUIButtonClick(2));
            
        
        }

        private void OnDisable() {
            startExercisesButton.onClick.RemoveAllListeners();
            song1Button.onClick.RemoveAllListeners();
            song2Button.onClick.RemoveAllListeners();
            song3Button.onClick.RemoveAllListeners();
            song4Button.onClick.RemoveAllListeners();
        }

        private void OnClickStartExercisesButton() {
            SceneLoader.Instance.LoadNewScene((int)SceneNames.Scene1);
        }

        private void OnClickBackToMainMenuButton() {
            SceneLoader.Instance.LoadNewScene((int)SceneNames.MainMenu);
        }

        private void OnClickSongButton(int songIndex) {
            _audioControl.BackgroundMusic(songIndex);
        }
        
        private void OnUIButtonClick(int audioClipIndex) {
            _audioControl.PlayAudioClip(audioClipIndex,1);
        }
    }
}
