using UnityEngine;
using UnityEngine.UIElements;


namespace Editor.Components.SettingsPage {
    public class Instructions : VisualElement
    {
        public Instructions() {
            var instructionStyle = Resources.Load<StyleSheet>("Styles/InstructionsMainStyle");
            var instructionUxml = Resources.Load<VisualTreeAsset>("InstructionsMainContainer");
            if (instructionStyle != null) {
                styleSheets.Add(instructionStyle);
            }
            if (instructionUxml != null) {
                instructionUxml.CloneTree(this);
                
            }else {
                Debug.LogError(
                    "Failed to load StyleSheet: BoxPlotStyle.uss.");
            }
        }
    
    }
}
