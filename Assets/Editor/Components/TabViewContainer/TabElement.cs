using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Components.TabViewContainer {
    [UxmlElement("Tab")]
    public partial class TabElement : Tab {
        public TabElement() {
            var asset = Resources.Load<VisualTreeAsset>("TabElement");
            if (asset == null) {
                Debug.LogError("Failed to load VisualTreeAsset: TabElement");
                return;
            }
            var root = new VisualElement();
            // Clone the asset into the root VisualElement without creating an extra TemplateContainer
            asset.CloneTree(root);

            // Add the contents of the root to 'this' to flatten the hierarchy
            foreach (var child in root.Children()) {
                Add(child);
            }
            
            
            // Load and apply stylesheet
            var tabElementStyleSheet = Resources.Load<StyleSheet>("Styles/TabElementStyle");
            if (tabElementStyleSheet != null) {
                styleSheets.Add(tabElementStyleSheet);
                AddToClassList("customTabElementStyle");
            } else {
                Debug.LogError("Failed to load StyleSheet: CenterRowContainerStyle.uss");
            }
        }
    }
}