using UnityEngine;
using UnityEngine.UIElements;


namespace Editor.Components.SettingsPage {
    public class Instructions : VisualElement {
       private VisualElement[] applicationSlides;
        private VisualElement[] audioSlides;

        private int applicationIndex = 0;
        private int audioIndex = 0;

        // Parent containers
        private VisualElement applicationContainer;
        private VisualElement audioContainer;
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

            // Query parent containers
            applicationContainer = this.Q<VisualElement>("ApplicationExplainContainer");
            audioContainer = this.Q<VisualElement>("AudioExplainContainer");
            questionButtonContainer = this.Q<VisualElement>("QuestionButtonContainer");
            backButtonContainer = this.Q<VisualElement>("BackButtonContainer");

            // Query container selection buttons
            var showApplicationButton = this.Q<Button>("ShowApplicationButton");
            var showAudioButton = this.Q<Button>("ShowAudioButton");

            // Query the back button
            backButton = this.Q<Button>("BackButton");
            if (backButton != null) {
                backButton.clicked += OnBackButtonClicked;
            }

            // Query application slides
            var slide1 = this.Q<VisualElement>("StartApplicationSlide");
            var slide2 = this.Q<VisualElement>("ExercisesSlider");
            var slide3 = this.Q<VisualElement>("DataSlider");
            applicationSlides = new[] { slide1, slide2, slide3 };
            // Initially, hide all slides in applicationSlides
            UpdateSlideVisibility(applicationSlides, applicationIndex);

            // Query audio slides
            var audioSlide1 = this.Q<VisualElement>("AudioSlide1");
            var audioSlide2 = this.Q<VisualElement>("AudioSlide2");
            // var audioSlide3 = this.Q<VisualElement>("AudioSlide3");
            audioSlides = new[] { audioSlide1, audioSlide2};
            // Initially, hide all slides in audioSlides
            UpdateSlideVisibility(audioSlides, audioIndex);

            // Initially show question buttons, hide the others
            if (questionButtonContainer != null) questionButtonContainer.style.display = DisplayStyle.Flex;
            if (applicationContainer != null) applicationContainer.style.display = DisplayStyle.None;
            if (audioContainer != null) audioContainer.style.display = DisplayStyle.None;

            // When QuestionButtonContainer is visible, BackButtonContainer should be hidden
            if (backButtonContainer != null) backButtonContainer.style.display = DisplayStyle.None;

            // Register container selection button callbacks
            if (showApplicationButton != null) {
                showApplicationButton.clicked += () => {
                    ShowContainer(applicationContainer, audioContainer, questionButtonContainer);
                    // Once shown, display the first application slide
                    UpdateSlideVisibility(applicationSlides, applicationIndex);
                };
            }

            if (showAudioButton != null) {
                showAudioButton.clicked += () => {
                    ShowContainer(audioContainer, applicationContainer, questionButtonContainer);
                    // Once shown, display the first audio slide
                    UpdateSlideVisibility(audioSlides, audioIndex);
                };
            }

            // Navigation buttons for application container
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

            // Navigation buttons for audio container
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
        }

        private void OnBackButtonClicked() {
            // Show QuestionButtonContainer, hide others
            if (questionButtonContainer != null) questionButtonContainer.style.display = DisplayStyle.Flex;
            if (applicationContainer != null) applicationContainer.style.display = DisplayStyle.None;
            if (audioContainer != null) audioContainer.style.display = DisplayStyle.None;

            // If QuestionButtonContainer is visible, BackButtonContainer should not be visible
            if (backButtonContainer != null) backButtonContainer.style.display = DisplayStyle.None;
        }

        private void ShowContainer(VisualElement toShow, VisualElement toHide, VisualElement questionButtons) {
            if (toShow != null) toShow.style.display = DisplayStyle.Flex;
            if (toHide != null) toHide.style.display = DisplayStyle.None;
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
    }
}