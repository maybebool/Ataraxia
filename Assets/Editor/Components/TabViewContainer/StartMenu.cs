using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Components.TabViewContainer {
    public class StartMenu : VisualElement
    {
        public StartMenu() {
            var startMenuStyle = Resources.Load<StyleSheet>("Styles/StartMenuStyle");
            var startMenuUxml = Resources.Load<VisualTreeAsset>("StartMenuContainer");
            if (startMenuStyle != null) {
                styleSheets.Add(startMenuStyle);
                AddToClassList("custom-start-menu-style");
            }
            if (startMenuUxml != null) {
                startMenuUxml.CloneTree(this);
                
            }else {
                Debug.LogError(
                    "Failed to load StyleSheet: BoxPlotStyle.uss.");
            }
        }
    }
}
