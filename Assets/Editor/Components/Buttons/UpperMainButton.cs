using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Components.Buttons {
    [UxmlElement("UpperMainButton")]
    public partial class UpperMainButton : Button {
        public UpperMainButton() {
            var asset = Resources.Load<VisualTreeAsset>("UpperMainButton");
            asset.CloneTree(this);

            var mainButtonStyle = Resources.Load<StyleSheet>("Styles/MainButtonStyle");
            if (mainButtonStyle != null) {
                styleSheets.Add(mainButtonStyle);
                AddToClassList("mainButtonStyle");
            }
            else {
                Debug.LogError("Failed to load StyleSheet: MainButtonStyle.uss");
            }
        }

        public UpperMainButton(Button button) {
            
        }
    }
}
