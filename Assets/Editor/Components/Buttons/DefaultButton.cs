using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Components.Buttons {
    [UxmlElement("DefaultButton")]
    public partial class DefaultButton : Button {
        public DefaultButton() {
            var asset = Resources.Load<VisualTreeAsset>("DefaultButton");
            asset.CloneTree(this);

            var defaultButton = Resources.Load<StyleSheet>("Styles/DefaultButtonStyle");
            if (defaultButton != null) {
                styleSheets.Add(defaultButton);
                AddToClassList("defaultButtonStyle");
            }
            else {
                Debug.LogError("Failed to load StyleSheet: DefaultButtonStyle.uss");
            }
        }

        public DefaultButton(Button button) {
            
        }
    }
}