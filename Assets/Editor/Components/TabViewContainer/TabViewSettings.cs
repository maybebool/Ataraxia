using Editor.Components.SettingsPage;
using UnityEngine;
using UnityEngine.UIElements;
using AudioSettings = Editor.Components.SettingsPage.AudioSettings;

namespace Editor.Components.TabViewContainer {
    public class TabViewSettings : TabView {
        public TabViewSettings() {
            
            var tabViewStyle = Resources.Load<StyleSheet>("Styles/TabViewStyle");
            if (tabViewStyle != null) {
                styleSheets.Add(tabViewStyle);
                AddToClassList("custom-TabView");
            }
            else {
                Debug.LogError("Failed to load StyleSheet: MainButtonStyle.uss");
            }
        
            var vETabContainer = this.Q<VisualElement>("unity-tab-view__content-container");
            vETabContainer?.Add(CreateAudioSettingsTab());
            vETabContainer?.Add(CreateTutorialTab());
            vETabContainer?.Add(CreateParametersSettingsTab());
        }

        private TabElement CreateAudioSettingsTab() {
            var audioTab = new TabElement {
                name = "audioTab",
                label = "Audio"
            };
            var audioSettings = new AudioSettings();
            audioTab.Add(audioSettings); 
            return audioTab;
        }
        
        private TabElement CreateParametersSettingsTab() {
            var parametersTab = new TabElement {
                name = "parameterTab",
                label = "Parameters"
            };
            var parametersSettings = new ParameterSettings();
            parametersTab.Add(parametersSettings); 
            return parametersTab;
        }
        
        private TabElement CreateTutorialTab() {
            var tutorialTab = new TabElement {
                name = "tutorialTab",
                label = "Instructions"
            };
            var instructions = new Instructions();
            tutorialTab.Add(instructions);
            return tutorialTab;
        }
    }
}
