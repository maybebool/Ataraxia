using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Components.CenterRowContainer {
    
    [UxmlElement("CenterRowContainer")]
    public partial class CenterRowContainer : VisualElement{
        
        public CenterRowContainer() {
            var asset = Resources.Load<VisualTreeAsset>("CenterRowContainer");
            asset.CloneTree(this);
        }

        public CenterRowContainer(VisualElement element) {
            
        } 
    }
}