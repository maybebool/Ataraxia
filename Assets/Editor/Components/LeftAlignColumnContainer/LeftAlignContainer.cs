using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Components.LeftAlignColumnContainer {
    [UxmlElement ("LeftAlignContainer")]
    public partial class LeftAlignContainer : VisualElement{
        public LeftAlignContainer() {
            var asset = Resources.Load<VisualTreeAsset>("LeftAlignContainer");
            if (asset == null) {
                Debug.LogError("Failed to load VisualTreeAsset: LeftAlignContainer");
                return;
            }

            // Create an instance of the VisualElement from the asset
            var root = new VisualElement();

            // Clone the asset into the root VisualElement without creating an extra TemplateContainer
            asset.CloneTree(root);

            // Add the contents of the root to 'this' to flatten the hierarchy
            foreach (var child in root.Children()) {
                this.Add(child);
            }

            // Load and apply stylesheet
            var leftAlignContainerStyle = Resources.Load<StyleSheet>("Styles/LeftAlignContainerStyle");
            if (leftAlignContainerStyle != null) {
                styleSheets.Add(leftAlignContainerStyle);
                AddToClassList("customLeftAlignContainerStyle");
            } else {
                Debug.LogError("Failed to load StyleSheet: LeftAlignContainerStyle.uss");
            }
        }
    }
}