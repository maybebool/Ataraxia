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
            var exTextForButton = new Label("Start/Pause the Exercise");
            var btnEx1 = new Buttons.SwitchButton(SceneNames.Exercise1);
            var btnEx2 = new Buttons.SwitchButton(SceneNames.Exercise2);
            var btnEx3 = new Buttons.SwitchButton(SceneNames.Exercise3);
            var btnEx4 = new Buttons.SwitchButton(SceneNames.Exercise4);
            var btnRestart = new Buttons.RestartButton();
            var container = new LeftAlignContainer();
            var vEContainerForStartTab = this.Q<VisualElement>("unity-tab-view__content-container");
            if (vEContainerForStartTab != null) {
                var menuTab = new TabElement {
                    name = $"MenuTab",
                    label = $"Start",
                };
                container.Add(exTextForButton);
                container.Add(btnEx1);
                menuTab.Add(container);
                vEContainerForStartTab.Add(menuTab);

                for (int i = 1; i <= 4; i++) {
                    var tab = new TabElement {
                        name = $"Tab{i}",
                        label = $"Exercise {i}"
                    };
                    vEContainerForStartTab.Add(tab);
                }
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

        private TabElement CreateStartTab(VisualElement element) {
            var menuTab = new TabElement{
                name = $"MenuTab",
                label = $"Start",
            };
            if (element != null) {
                var appTextForButton = new Label("Start the Application");
                var quitTextForButton = new Label("Quit the Application");
                var btnEx1 = new Buttons.SwitchButton(SceneNames.MainMenu);
                // var btnEx1 = new Buttons.SwitchButton(SceneNames.MainMenu);
                var container1 = new LeftAlignContainer();
                var container2 = new LeftAlignContainer();
                container1.Add(appTextForButton);
                container1.Add(btnEx1);
                menuTab.Add(container1);

            }
            return menuTab;
        }
    }
}
