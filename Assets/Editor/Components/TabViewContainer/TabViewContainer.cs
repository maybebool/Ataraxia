using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Components.TabViewContainer {
    [UxmlElement ("TabView")]
    public partial class TabViewContainer : TabView {
        public TabViewContainer() {
            var asset = Resources.Load<VisualTreeAsset>("TabView");
            if (asset == null) {
                Debug.LogError("Failed to load VisualTreeAsset: TabView");
                return;
            }
            
            asset.CloneTree(this);
            
            var vEContainer = this.Q<VisualElement>("unity-tab-view__content-container");
            if (vEContainer != null) {
                var menuTab = new TabElement {
                    name = $"MenuTab",
                    label = $"Start"
                };
                vEContainer.Add(menuTab);
                
                for (int i = 1; i <= 4; i++) {
                    var tab = new TabElement {
                        name = $"Tab{i}",
                        label = $"Exercise {i}"
                    };
                    vEContainer.Add(tab);
                }
            } else {
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
        
        
    }
}