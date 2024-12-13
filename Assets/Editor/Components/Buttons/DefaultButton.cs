using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Components.Buttons {
    
    public class DefaultButton : Button {
        private Texture2D backgroundImage;
        public DefaultButton(string title) {
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
            
            backgroundImage = Resources.Load<Texture2D>("Images/ButtonBackground");
            style.backgroundImage = backgroundImage;
            text = title;
        }

        public DefaultButton(Button button) {
            
        }
    }
}