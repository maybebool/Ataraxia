using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Components.CenterRowContainer {
    
    [UxmlElement("CenterRowContainer")]
    public partial class CenterRowContainer : VisualElement{
        
        public CenterRowContainer() {
            
            var asset = Resources.Load<VisualTreeAsset>("CenterRowContainer");
            if (asset == null) {
                Debug.LogError("Failed to load VisualTreeAsset: CenterRowContainer");
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

            // Instantiate the buttons
            var button1 = new UpperMainButton.UpperMainButton();
            var button2 = new UpperMainButton.UpperMainButton();
            var button3 = new UpperMainButton.UpperMainButton();

            // Add buttons directly to 'this' to make them direct children of the root
            this.Add(button1);
            this.Add(button2);
            this.Add(button3);

            // Load and apply stylesheet
            var centerRowContainerStyle = Resources.Load<StyleSheet>("Styles/CenterRowContainerStyle");
            if (centerRowContainerStyle != null) {
                styleSheets.Add(centerRowContainerStyle);
                AddToClassList("customContainerRow");
            } else {
                Debug.LogError("Failed to load StyleSheet: CenterRowContainerStyle.uss");
            }
            // var asset = Resources.Load<VisualTreeAsset>("CenterRowContainer");
            // var centerRowContainer = Resources.Load<StyleSheet>("Styles/CenterRowContainerStyle");
            // if (centerRowContainer != null) {
            //     styleSheets.Add(centerRowContainer);
            //     AddToClassList("customContainerRow");
            // }
            // else {
            //     Debug.LogError("Failed to load StyleSheet: MainButtonStyle.uss");
            // }
            // var button = new UpperMainButton.UpperMainButton();
            // var button2 = new UpperMainButton.UpperMainButton();
            // var button3 = new UpperMainButton.UpperMainButton();
            // var root = asset.CloneTree();
            // root.Add(button);
            // root.Add(button2);
            // root.Add(button3);
            // Add(root);
            
            
        }

        public CenterRowContainer(VisualElement element) {
            
        } 
    }
}