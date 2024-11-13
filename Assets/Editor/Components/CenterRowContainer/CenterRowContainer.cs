using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Components.CenterRowContainer {
    
    [UxmlElement("CenterRowContainer")]
    public partial class CenterRowContainer : VisualElement{
        [HideInInspector] public UpperMainButton.UpperMainButton button1 = new();
        [HideInInspector] public UpperMainButton.UpperMainButton button2 = new();
        [HideInInspector] public UpperMainButton.UpperMainButton button3 = new();
        
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
            // button1.Q<Button>("Scene Manager");
            // button2.Q<Button>("Data View");
            // button3.Q<Button>("Settings");
            button1.text = "Scene Manager";
            button2.text = "Data View";
            button3.text = "Settings";

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
        }

        public CenterRowContainer(VisualElement element) {
            
        } 
    }
}