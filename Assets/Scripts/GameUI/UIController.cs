using Audio;
using SceneHandling;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Utils;

namespace GameUI {
    public class UIController : MonoBehaviour {
    
        [Header("Main Menu Home Panel")]
        [SerializeField] private GameObject homePanel;
        [SerializeField] private Button mainButtonHome;
        [SerializeField] private Button mainButtonSettings;
        [SerializeField] private Button mainButtonTutorials;
        [SerializeField] private Button startExercisesButton;
        [SerializeField] private Button quitButton;
        
        [Header("Main Menu Settings Panel")]
        [SerializeField] private GameObject settingsPanel;
        [SerializeField] private Toggle audioToggleOnOff;
        [SerializeField] private Button song1Button;
        [SerializeField] private Button song2Button;
        [SerializeField] private Button song3Button;
        [SerializeField] private Button song4Button;
        [SerializeField] private Button saveChangesButton;
        [SerializeField] private Button defaultAudioSettingsButton;
        
        [Header("Main Menu Tutorial Panel")]
        [SerializeField] private GameObject tutorialPanel;
        [SerializeField] private GameObject tutorialExercisesPanel;
        [SerializeField] private GameObject tutorialBindingsPanel;
        [SerializeField] private Button exercisesTutorialButton;
        [SerializeField] private Button bindingsTutorialButton;
        [SerializeField] private Button nextTutorialExerciseSlideButton;
        [SerializeField] private Button previousTutorialExerciseSlideButton;
        [SerializeField] private GameObject bindingImage;
        [SerializeField] private GameObject[] tutorialSlides;
        private int _currentSlideIndex = 0;
        private Profiles _audioMixerProfileSo;

        private void Awake() {
            if (AudioController.Instance == null) {
                Debug.LogError("AudioController not found in the scene.");
            }

            if (_audioMixerProfileSo != null) {
                _audioMixerProfileSo.SetProfile(_audioMixerProfileSo);
            }
            
            for (int i = 0; i < tutorialSlides.Length; i++) {
                tutorialSlides[i].SetActive(i == 0);
            }
            _currentSlideIndex = 0;
        }
        
        private void OnEnable() {
            // home panel
            UIUtil.CallMultipleActions(startExercisesButton, ()=>OnClickStartASceneButton(SceneNames.Exercise1), ()=>OnClickUIButton(0));
            UIUtil.CallMultipleActions(mainButtonHome,OnClickStartPanel, ()=>OnClickUIButton(0));
            UIUtil.CallMultipleActions(quitButton,OnClickQuitButton, ()=>OnClickUIButton(0));
            // settings panel
            UIUtil.CallMultipleActions(song1Button,()=>OnClickSongButton(0), ()=>OnClickUIButton(0));
            UIUtil.CallMultipleActions(song2Button,()=>OnClickSongButton(1), ()=>OnClickUIButton(0));
            UIUtil.CallMultipleActions(song3Button,()=>OnClickSongButton(2), ()=>OnClickUIButton(0));
            UIUtil.CallMultipleActions(song4Button,()=>OnClickSongButton(3), ()=>OnClickUIButton(0));
            UIUtil.CallMultipleActions(mainButtonSettings,OnClickSettingsPanel, ()=>OnClickUIButton(0));
            UIUtil.CallMultipleActions(saveChangesButton,OnClickSaveSettings, ()=>OnClickUIButton(0));
            UIUtil.CallMultipleActions(defaultAudioSettingsButton,OnClickDefaultSettings, ()=>OnClickUIButton(0));
            // tutorial panel
            UIUtil.CallMultipleActions(mainButtonTutorials,OnClickTutorialPanel, ()=>OnClickUIButton(0));
            UIUtil.CallMultipleActions(exercisesTutorialButton,OnClickTutorialExercisesPanel, ()=>OnClickUIButton(0));
            UIUtil.CallMultipleActions(bindingsTutorialButton,OnClickTutorialBindingsPanel, ()=>OnClickUIButton(0));
            UIUtil.CallMultipleActions(nextTutorialExerciseSlideButton, OnClickNextTutorialExerciseSlide, ()=>OnClickUIButton(0));
            UIUtil.CallMultipleActions(previousTutorialExerciseSlideButton, OnClickPreviousTutorialExerciseSlide, ()=>OnClickUIButton(0));
            
        }

        private void OnDisable() {
            // home panel
            startExercisesButton.onClick.RemoveAllListeners();
            mainButtonHome.onClick.RemoveAllListeners();
            quitButton.onClick.RemoveAllListeners();
            // settings panel
            mainButtonSettings.onClick.RemoveAllListeners();
            song1Button.onClick.RemoveAllListeners();
            song2Button.onClick.RemoveAllListeners();
            song3Button.onClick.RemoveAllListeners();
            song4Button.onClick.RemoveAllListeners();
            // tutorial panel
            mainButtonTutorials.onClick.RemoveAllListeners();
            nextTutorialExerciseSlideButton.onClick.RemoveAllListeners();
            previousTutorialExerciseSlideButton.onClick.RemoveAllListeners();

        }

        private void OnClickStartASceneButton(SceneNames scene) {
            SceneLoader.Instance.LoadNewScene(scene);
        }

        private void OnClickSongButton(int songIndex) {
            AudioController.Instance.BackgroundMusic(songIndex);
        }
        
        private void OnClickUIButton(int audioClipIndex) {
            AudioController.Instance.PlayAudioClip(audioClipIndex,1);
        }

        private void OnClickSaveSettings() {
            if (Settings.profile && Settings.profile.audioMixer != null) {
                Settings.profile.SaveAudioLevels();
            }
        }

        private void OnClickDefaultSettings() {
            AudioController.Instance.CancelChanges();
        }

        private void OnClickStartPanel() {
            homePanel.SetActive(true);
            settingsPanel.SetActive(false);
            tutorialPanel.SetActive(false);
            bindingImage.SetActive(false);
        }
        
        private void OnClickSettingsPanel() {
            settingsPanel.SetActive(true);
            homePanel.SetActive(false);
            tutorialPanel.SetActive(false);
            bindingImage.SetActive(false);
        }
        
        private void OnClickTutorialPanel() {
            tutorialPanel.SetActive(true);
            settingsPanel.SetActive(false);
            homePanel.SetActive(false);
            bindingImage.SetActive(false);
        }
        
        private void OnClickTutorialExercisesPanel() {
            tutorialExercisesPanel.SetActive(true);
            bindingImage.SetActive(false);
            tutorialBindingsPanel.SetActive(false);
        }
        
        private void OnClickTutorialBindingsPanel() {
            tutorialBindingsPanel.SetActive(true);
            bindingImage.SetActive(true);
            tutorialExercisesPanel.SetActive(false);
        }
        
        private void OnClickNextTutorialExerciseSlide() {
            tutorialSlides[_currentSlideIndex].SetActive(false);
            _currentSlideIndex = (_currentSlideIndex + 1) % tutorialSlides.Length;
            tutorialSlides[_currentSlideIndex].SetActive(true);
        }
        
        private void OnClickPreviousTutorialExerciseSlide() {
            tutorialSlides[_currentSlideIndex].SetActive(false);
            _currentSlideIndex = (_currentSlideIndex - 1 + tutorialSlides.Length) % tutorialSlides.Length;
            tutorialSlides[_currentSlideIndex].SetActive(true);
        }
        
        private void OnClickQuitButton() {
            Application.Quit();
        }
    }
}
