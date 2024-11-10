using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.CustomEditorWindow.Components.Background {
    [UxmlElement("Background")]
    public partial class BackgroundElements : VisualElement {
        private VisualElement backgroundImage { get; set; }

        public BackgroundElements() {
            var asset = Resources.Load<VisualTreeAsset>("BackgroundElements");
            asset.CloneTree(this);
            backgroundImage = this.Q("backgroundImage");
        }

        public void SetBackGroundIMage(Texture2D image) {
            backgroundImage.style.backgroundImage = image;
        }
    }
}