using UnityEngine;
using UnityEngine.UIElements;


namespace Editor.Components.SettingsPage {
    public class Instructions : VisualElement { 
        private VisualElement[] applicationSlides;
        private VisualElement[] audioSlides;
        private VisualElement[] dataSlides;
        private VisualElement[] exercisesSlides;
        
        private int applicationIndex = 0;
        private int audioIndex = 0;
        private int dataIndex = 0;
        private int exercisesIndex = 0;
        
        private VisualElement applicationContainer;
        private VisualElement audioContainer;
        private VisualElement dataContainer;
        private VisualElement exercisesContainer;
        private VisualElement questionButtonContainer;
        private VisualElement backButtonContainer; // Added
        private Button backButton; // Added

        public Instructions() {
            var instructionStyle = Resources.Load<StyleSheet>("Styles/InstructionsMainStyle");
            var instructionUxml = Resources.Load<VisualTreeAsset>("InstructionsMainContainer");
            if (instructionStyle != null) {
                styleSheets.Add(instructionStyle);
                AddToClassList("instructions-style");
            }

            if (instructionUxml != null) {
                instructionUxml.CloneTree(this);
            } else {
                Debug.LogError("Failed to load InstructionsMainContainer.");
            }
            
            applicationContainer = this.Q<VisualElement>("ApplicationExplainContainer");
            audioContainer = this.Q<VisualElement>("AudioExplainContainer");
            dataContainer = this.Q<VisualElement>("DataExplainContainer");
            exercisesContainer = this.Q<VisualElement>("ExercisesExplainContainer");
            questionButtonContainer = this.Q<VisualElement>("QuestionButtonContainer");
            backButtonContainer = this.Q<VisualElement>("BackButtonContainer");
            
            var showApplicationButton = this.Q<Button>("ShowApplicationButton");
            var showAudioButton = this.Q<Button>("ShowAudioButton");
            var showDataButton = this.Q<Button>("ShowDataButton");
            var showExercisesButton = this.Q<Button>("ShowExercisesButton");
            
            backButton = this.Q<Button>("BackButton");
            if (backButton != null) {
                backButton.clicked += OnBackButtonClicked;
            }
            
            var slide1 = this.Q<VisualElement>("StartApplicationSlide");
            var slide2 = this.Q<VisualElement>("ExercisesSlider");
            var slide3 = this.Q<VisualElement>("DataSlider");
            applicationSlides = new[] { slide1, slide2, slide3 };
            UpdateSlideVisibility(applicationSlides, applicationIndex);
            
            var audioSlide1 = this.Q<VisualElement>("AudioSlide1");
            var audioSlide2 = this.Q<VisualElement>("AudioSlide2");
            audioSlides = new[] { audioSlide1, audioSlide2};
            UpdateSlideVisibility(audioSlides, audioIndex);
            
            var dataSlide1 = this.Q<VisualElement>("DataSlide1");
            var dataSlide2 = this.Q<VisualElement>("DataSlide2");
            var dataSlide3 = this.Q<VisualElement>("DataSlide3");
            dataSlides = new[] { dataSlide1, dataSlide2, dataSlide3 };
            UpdateSlideVisibility(dataSlides, dataIndex);
            
            var exercisesSlide1 = this.Q<VisualElement>("ExerciseSlide1");
            var exercisesSlide2 = this.Q<VisualElement>("ExerciseSlide2");
            var exercisesSlide3 = this.Q<VisualElement>("ExerciseSlide3");
            exercisesSlides = new[] { exercisesSlide1, exercisesSlide2, exercisesSlide3 };
            UpdateSlideVisibility(exercisesSlides, dataIndex);
            
            if (questionButtonContainer != null) questionButtonContainer.style.display = DisplayStyle.Flex;
            if (applicationContainer != null) applicationContainer.style.display = DisplayStyle.None;
            if (audioContainer != null) audioContainer.style.display = DisplayStyle.None;
            if (backButtonContainer != null) backButtonContainer.style.display = DisplayStyle.None;
            
            if (showApplicationButton != null) {
                showApplicationButton.clicked += () => {
                    ShowContainer(applicationContainer, questionButtonContainer);
                    UpdateSlideVisibility(applicationSlides, applicationIndex);
                };
            }

            if (showAudioButton != null) {
                showAudioButton.clicked += () => {
                    ShowContainer(audioContainer, questionButtonContainer);
                    UpdateSlideVisibility(audioSlides, audioIndex);
                };
            }

            if (showDataButton != null) {
                showDataButton.clicked += () => {
                    ShowContainer(dataContainer, questionButtonContainer);
                    UpdateSlideVisibility(dataSlides, exercisesIndex);
                };
            }

            if (showExercisesButton != null) {
                showExercisesButton.clicked += () => {
                    ShowContainer(exercisesContainer, questionButtonContainer);
                    UpdateSlideVisibility(exercisesSlides, exercisesIndex);
                };
                
            }
            
            HandleSliderButtonEvents();
        }

        private void OnBackButtonClicked() {
            // Show QuestionButtonContainer, hide others
            if (questionButtonContainer != null) questionButtonContainer.style.display = DisplayStyle.Flex;
            if (applicationContainer != null) applicationContainer.style.display = DisplayStyle.None;
            if (audioContainer != null) audioContainer.style.display = DisplayStyle.None;
            if (dataContainer != null) dataContainer.style.display = DisplayStyle.None;
            if (exercisesContainer != null) exercisesContainer.style.display = DisplayStyle.None;
            
            if (backButtonContainer != null) backButtonContainer.style.display = DisplayStyle.None;
        }

        private void ShowContainer(VisualElement toShow, VisualElement questionButtons) {
            if (toShow != null) toShow.style.display = DisplayStyle.Flex;
            if (questionButtons != null) questionButtons.style.display = DisplayStyle.None;
        
            // When we show a container (either application or audio), we want the backButtonContainer visible
            if (backButtonContainer != null) backButtonContainer.style.display = DisplayStyle.Flex;
        }

        private void MoveToNextSlide(ref int currentIndex, VisualElement[] slides) {
            currentIndex = (currentIndex + 1) % slides.Length;
            UpdateSlideVisibility(slides, currentIndex);
        }

        private void MoveToPrevSlide(ref int currentIndex, VisualElement[] slides) {
            currentIndex = (currentIndex - 1 + slides.Length) % slides.Length;
            UpdateSlideVisibility(slides, currentIndex);
        }

        private void UpdateSlideVisibility(VisualElement[] slides, int currentIndex) {
            for (int i = 0; i < slides.Length; i++) {
                if (slides[i] != null) {
                    slides[i].style.display = (i == currentIndex) ? DisplayStyle.Flex : DisplayStyle.None;
                }
            }
        }

        private void HandleSliderButtonEvents() {
            var appPrevButton = this.Q<Button>("AppPrevButton");
            var appNextButton = this.Q<Button>("AppNextButton");
            if (appPrevButton != null) {
                appPrevButton.clicked += () => {
                    MoveToPrevSlide(ref applicationIndex, applicationSlides);
                };
            }
            if (appNextButton != null) {
                appNextButton.clicked += () => {
                    MoveToNextSlide(ref applicationIndex, applicationSlides);
                };
            }
            
            var audioPrevButton = this.Q<Button>("AudioPrevButton");
            var audioNextButton = this.Q<Button>("AudioNextButton");
            if (audioPrevButton != null) {
                audioPrevButton.clicked += () => {
                    MoveToPrevSlide(ref audioIndex, audioSlides);
                };
            }
            if (audioNextButton != null) {
                audioNextButton.clicked += () => {
                    MoveToNextSlide(ref audioIndex, audioSlides);
                };
            }
            
            var dataPrevButton = this.Q<Button>("DataPrevButton");
            var dataNextButton = this.Q<Button>("DataNextButton");
            if (dataPrevButton != null) {
                dataPrevButton.clicked += () => {
                    MoveToPrevSlide(ref dataIndex, dataSlides);
                };
            }
            if (dataNextButton != null) {
                dataNextButton.clicked += () => {
                    MoveToNextSlide(ref dataIndex, dataSlides);
                };
            }
            
            var exercisesPrevButton = this.Q<Button>("ExercisePrevButton");
            var exercisesNextButton = this.Q<Button>("ExerciseNextButton");
            if (exercisesPrevButton != null) {
                exercisesPrevButton.clicked += () => {
                    MoveToPrevSlide(ref exercisesIndex, exercisesSlides);
                };
            }
            if (exercisesPrevButton != null) {
                exercisesNextButton.clicked += () => {
                    MoveToNextSlide(ref exercisesIndex, exercisesSlides);
                };
            }
        }
    }
}