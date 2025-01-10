using Editor.Components.Buttons;
using Editor.Components.Graphs;
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
            
            // var menuTabView = new StartMenu();
            // var startButton = new StartButton(SceneNames.MainMenu);
            // var startContainer = menuTabView.Q<VisualElement>("StartApplicationContainer");
            // startContainer.Add(startButton);
            var circle = new CircleGraph("Test") {
                CircleDegree = 270f
            };
            // menuTab.Add(menuTabView);
            menuTab.Add(circle);
            return menuTab;
        }

        private TabElement CreateExerciseTab(int exerciseNumber,SceneNames scene) {
            var exerciseTab = new TabElement {
                name = "ExerciseTab " + exerciseNumber,
                label = "Exercise " + exerciseNumber,
            };
            var exerciseTabView = new ExerciseTabView();
            var startButton = new StartButton(scene);
            var startContainer = exerciseTabView.Q<VisualElement>("StartApplicationContainer");
            startContainer.Add(startButton);
            exerciseTab.Add(exerciseTabView);
            return exerciseTab;

        }
        
    }
}
