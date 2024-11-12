using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.CustomEditorWindow.Components.UpperMainButton {
    [UxmlElement("UpperMainButton")]
    public partial class UpperMainButton : Button {
        
        
        public UpperMainButton() {
            var asset = Resources.Load<VisualTreeAsset>("UpperMainButton");
            asset.CloneTree(this);
        }

        public UpperMainButton(Button button) {
            
        }
    }
}
