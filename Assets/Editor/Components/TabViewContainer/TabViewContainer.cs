using Editor.Components.LeftAlignColumnContainer;
using GameUI;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Components.TabViewContainer {
    [UxmlElement("TabView")]
    public partial class TabViewContainer : TabView {
        public TabViewContainer() {
            var asset = Resources.Load<VisualTreeAsset>("TabView");
            if (asset == null) {
                Debug.LogError("Failed to load VisualTreeAsset: TabView");
                return;
            }

            asset.CloneTree(this);
            
            var vETabContainer = this.Q<VisualElement>("unity-tab-view__content-container");
            if (vETabContainer != null) {
                vETabContainer.Add(CreateStartTab());
                vETabContainer.Add(CreateExerciseTab(1,SceneNames.Exercise1));
                vETabContainer.Add(CreateExerciseTab(2,SceneNames.Exercise2));
                vETabContainer.Add(CreateExerciseTab(3,SceneNames.Exercise3));
                vETabContainer.Add(CreateExerciseTab(4,SceneNames.Exercise4));
            }
            else {
                Debug.LogError("Failed to find VisualElement with id 'visual_element_number_1'");
            }



            var tabViewStyle = Resources.Load<StyleSheet>("Styles/TabViewStyle");
            if (tabViewStyle != null) {
                styleSheets.Add(tabViewStyle);
                AddToClassList("custom-TabView");
            }
            else {
                Debug.LogError("Failed to load StyleSheet: MainButtonStyle.uss");
            }
        }

        private TabElement CreateStartTab() {
            var menuTab = new TabElement {
                name = "MenuTab",
                label = "Start",
            };
            
            var menuTabView = new StartMenu();
            menuTab.Add(menuTabView);
            return menuTab;
        }

        private TabElement CreateExerciseTab(int exerciseNumber,SceneNames scene) {
            var exerciseTab = new TabElement {
                name = "ExerciseTab " + exerciseNumber,
                label = "Exercise " + exerciseNumber,
            };
            var startPauseText = new Label("Start/Pause the Exercise");
            var restartExercise = new Label("Restart the Exercise");
            var quitTextForButton = new Label("Quit the Application");
            var btnEx = new Buttons.StartButton(scene);
            var pauseResumeToggle = new Buttons.PauseResumeToggle();
            var btnRestart = new Buttons.RestartButton();
            var btnQuit = new Buttons.QuitButton();
            var container1 = new LeftAlignContainer();
            var containerPause = new LeftAlignContainer();
            var container2 = new LeftAlignContainer();
            var container3 = new LeftAlignContainer();
            
            container1.Add(startPauseText);
            container1.Add(btnEx);
            containerPause.Add(pauseResumeToggle);
            container2.Add(restartExercise);
            container2.Add(btnRestart);
            container3.Add(quitTextForButton);
            container3.Add(btnQuit);
            exerciseTab.Add(container1);
            exerciseTab.Add(containerPause);
            exerciseTab.Add(container2);
            exerciseTab.Add(container3);
            return exerciseTab;

        }
        
    }
}
