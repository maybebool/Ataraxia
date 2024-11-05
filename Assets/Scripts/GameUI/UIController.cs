using Audio;
using SceneHandling;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace GameUI {
    public class UIController : MonoBehaviour {
    
        [Header("Main Menu Home Buttons")]
        [SerializeField] private Button mainButtonHome;
        [SerializeField] private Button mainButtonSettings;
        [SerializeField] private Button mainButtonTutorials;
        [SerializeField] private Button startExercisesButton;
        
        [Header("Main Menu Settings Buttons")]
        [SerializeField] private Toggle audioToggleOnOff;
        [SerializeField] private Button song1Button;
        [SerializeField] private Button song2Button;
        [SerializeField] private Button song3Button;
        [SerializeField] private Button song4Button;
        [SerializeField] private Button saveChangesButton;
        [SerializeField] private Button defaultAudioSettingsButton;
        [SerializeField] private GameObject homePanel;
        [SerializeField] private GameObject settingsPanel;
        [SerializeField] private GameObject tutorialPanel;
        
        
        public AudioController _audioControl;
        private Profiles audioMixerProfileSo;

        private void Awake() {
            if (_audioControl == null) {
                _audioControl = FindObjectOfType<AudioController>();
                if (_audioControl == null) {
                    Debug.LogError("AudioController not found in the scene.");
                }
            }

            if (audioMixerProfileSo != null) {
                audioMixerProfileSo.SetProfile(audioMixerProfileSo);
            }
        }
        
        private void OnEnable() {
            UIUtil.CallMultipleActions(startExercisesButton, OnClickStartExercisesButton, ()=>OnClickUIButton(2));
            UIUtil.CallMultipleActions(song1Button,()=>OnClickSongButton(0), ()=>OnClickUIButton(2));
            UIUtil.CallMultipleActions(song2Button,()=>OnClickSongButton(1), ()=>OnClickUIButton(2));
            UIUtil.CallMultipleActions(song3Button,()=>OnClickSongButton(2), ()=>OnClickUIButton(2));
            UIUtil.CallMultipleActions(song4Button,()=>OnClickSongButton(0), ()=>OnClickUIButton(2));
            UIUtil.CallMultipleActions(mainButtonHome,OnClickStartPanel, ()=>OnClickUIButton(2));
            UIUtil.CallMultipleActions(mainButtonTutorials,OnClickTutorialPanel, ()=>OnClickUIButton(2));
            UIUtil.CallMultipleActions(mainButtonSettings,OnClickSettingsPanel, ()=>OnClickUIButton(2));
            UIUtil.CallMultipleActions(saveChangesButton,OnClickSaveSettings, ()=>OnClickUIButton(2));
            UIUtil.CallMultipleActions(defaultAudioSettingsButton,OnClickDefaultSettings, ()=>OnClickUIButton(2));
            
        
        }

        private void OnDisable() {
            startExercisesButton.onClick.RemoveAllListeners();
            song1Button.onClick.RemoveAllListeners();
            song2Button.onClick.RemoveAllListeners();
            song3Button.onClick.RemoveAllListeners();
            song4Button.onClick.RemoveAllListeners();
            mainButtonHome.onClick.RemoveAllListeners();
            mainButtonSettings.onClick.RemoveAllListeners();
            mainButtonTutorials.onClick.RemoveAllListeners();
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
        
        private void OnClickUIButton(int audioClipIndex) {
            _audioControl.PlayAudioClip(audioClipIndex,1);
        }

        private void OnClickSaveSettings() {
            if (Settings.profile && Settings.profile.audioMixer != null) {
                Settings.profile.SaveAudioLevels();
            }
        }

        private void OnClickDefaultSettings() {
            _audioControl.CancelChanges();
        }

        private void OnClickStartPanel() {
            homePanel.SetActive(true);
            settingsPanel.SetActive(false);
            tutorialPanel.SetActive(false);
        }
        
        private void OnClickSettingsPanel() {
            settingsPanel.SetActive(true);
            homePanel.SetActive(false);
            tutorialPanel.SetActive(false);
        }
        
        private void OnClickTutorialPanel() {
            tutorialPanel.SetActive(true);
            settingsPanel.SetActive(false);
            homePanel.SetActive(false);
        }
    }
}
