using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.CustomEditorWindow.Components.Background {
    [UxmlElement("Background")]
    public partial class BackgroundElements : VisualElement {

        private VisualElement backgroundImage => this.Q("background");
        
        
        public BackgroundElements() {}
        public BackgroundElements(Texture2D image)
        {
            // It assumes the UXML file is called "CardElement.uxml" and 
            // is placed at the "Resources" folder.
            var asset = Resources.Load<VisualTreeAsset>("BackgroundElements");
            asset.CloneTree(this);

            backgroundImage.style.backgroundImage = image;
        }
    }
}