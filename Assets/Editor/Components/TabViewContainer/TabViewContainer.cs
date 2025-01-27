using Editor.Components.Buttons;
using GameUI;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Components.TabViewContainer {
    [UxmlElement("TabView")]
    public partial class TabViewContainer : TabView {
        
        #region Text fields
        private const string Ex1Title = "Leg/Head Tremor Detection";
        private const string Ex2Title = "Hand Tremor Detection";
        private const string Ex3Title = "Finger Tone Detection";
        
        private const string Ex1Text = "In this exercise, the user’s leg and head movement patterns are examined for tremor behavior. \n" +
                                       "\n1. The user should pick up the controllers and rest them on their legs, " +
                                       "pressing and holding the red-marked buttons (as shown in the image). \n" +
                                       "\n2. A visible beam is emitted from each controller. During the exercise, " +
                                       "the user should try to keep each beam within a circle while keeping their " +
                                       "hands resting on their legs. \n" +
                                       "\n3. The user sees an object in their line of sight that follows their gaze. " +
                                       "During the exercise, multiple objects approach the user. " +
                                       "The goal is to avoid obstacles and capture the target objects with the gaze-tracking object.\n" +
                                       "\n4. Once the exercise is finished, the next exercise starts automatically.";
        
        
        private const string Ex2Text = "In this exercise, the user’s hand movement patterns are analyzed, " +
                                       "focusing on tremor movements in the hands.\n" +
                                       "\n1. The user must hold down the red-marked buttons for the entire duration of the exercise.\n" +
                                       "\n2. Using the blue-marked buttons, the user can lift and move the objects in the scene.\n" +
                                       "\n3. The exercise involves picking up and holding the respective objects.\n" +
                                       "\n4. Next, the user follows the objects in the scene with the ones held " +
                                       "in their hands and imitates their movements.\n" +
                                       "\n5. At the end, the objects must be deposited in the designated area.\n" +
                                       "\n6. After step 5, the next exercise starts automatically.\n";
        
        private const string Ex3Text = "In this exercise, the user’s fine motor skills are tested for muscle tone in the fingers.\n" +
                                       "\n1. Using the red-marked buttons shown in the image, the user can move the " +
                                       "objects on the left and right up or down.\n" +
                                       "\n2. The more the button is pressed, the higher the object rises. The less " +
                                       "the button is pressed, the more the object lowers.\n" +
                                       "\n3. The object in the middle moves up and down on its own.\n" +
                                       "\n4. The user must try to keep the two objects on the left and right at " +
                                       "roughly the same height as the middle object.\n" +
                                       "\n5. If the height difference becomes too great, points are deducted. " +
                                       "If the difference is kept minimal, points can be regained over time.\n" +
                                       "\n6. When the exercise is finished, the user automatically returns to the main menu.\n";
        #endregion
        
        public TabViewContainer() {
            var asset = Resources.Load<VisualTreeAsset>("TabView");
            if (asset == null) {
                Debug.LogError("Failed to load VisualTreeAsset: TabView");
                return;
            }
            asset.CloneTree(this);
            
            var tabViewStyle = Resources.Load<StyleSheet>("Styles/TabViewStyle");
            if (tabViewStyle != null) {
                styleSheets.Add(tabViewStyle);
                AddToClassList("custom-TabView");
            }
            else {
                Debug.LogError("Failed to load StyleSheet: MainButtonStyle.uss");
            }
            
            var smallImageEx1 = Resources.Load<Texture2D>("Images/RedControllerExercise1");
            var smallImageEx2 = Resources.Load<Texture2D>("Images/RedBlueControllerExercise2");
            var smallImageEx3 = Resources.Load<Texture2D>("Images/RedControllerExercise3");
            
            var largeImageEx1 = Resources.Load<Texture2D>("Images/LargeImageExercise1");
            var largeImageEx2 = Resources.Load<Texture2D>("Images/LargeImageExercise2");
            var largeImageEx3 = Resources.Load<Texture2D>("Images/LargeImageExercise3");
            
            var vETabContainer = this.Q<VisualElement>("unity-tab-view__content-container");
            if (vETabContainer != null) {
                vETabContainer.Add(CreateStartTab());
                vETabContainer.Add(CreateExerciseTab(1,SceneNames.Exercise1,Ex1Title,Ex1Text, smallImageEx1, largeImageEx1));
                vETabContainer.Add(CreateExerciseTab(2,SceneNames.Exercise2,Ex2Title,Ex2Text, smallImageEx2, largeImageEx2));
                vETabContainer.Add(CreateExerciseTab(3,SceneNames.Exercise3,Ex3Title,Ex3Text,smallImageEx3, largeImageEx3));
            }
            else {
                Debug.LogError("Failed to find VisualElement with id 'visual_element_number_1'");
            }
        }

        private TabElement CreateStartTab() {
            var menuTab = new TabElement {
                name = "MenuTab",
                label = "Start",
            };
            
            var menuTabView = new StartMenu();
            var startButton = new StartButton(SceneNames.MainMenu);
            var startContainer = menuTabView.Q<VisualElement>("StartApplicationContainer");
            startContainer.Add(startButton);
           
            menuTab.Add(menuTabView);
            
            return menuTab;
        }

        private TabElement CreateExerciseTab(int exerciseNumber,SceneNames scene, string title,string text, Texture2D smallImage, Texture2D largeImage) {
            var exerciseTab = new TabElement {
                name = "ExerciseTab " + exerciseNumber,
                label = "Exercise " + exerciseNumber,
            };
            var exerciseTabView = new ExerciseTabView();
            var startButton = new StartButton(scene);
            var startContainer = exerciseTabView.Q<VisualElement>("StartApplicationContainer");
            var smallImageElement = exerciseTabView.Q<VisualElement>("ExerciseImageSmall");
            var largeImageElement = exerciseTabView.Q<VisualElement>("ImageContainer");
            var titleLabel = exerciseTabView.Q<Label>("TitleLabel");
            var textLabel = exerciseTabView.Q<Label>("TextLabel");
            titleLabel.text = title;
            textLabel.text = text;
            
            smallImageElement.style.backgroundImage = smallImage;
            largeImageElement.style.backgroundImage = largeImage;
            startContainer.Add(startButton);
            exerciseTab.Add(exerciseTabView);
            return exerciseTab;

        }
        
    }
}
