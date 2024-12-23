using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Components.TabViewContainer {
    public class DataGraphTab : VisualElement
    {
        public DataGraphTab(){
            var dataStyle = Resources.Load<StyleSheet>("Styles/DataGraphsStyle");
            var dataUxml = Resources.Load<VisualTreeAsset>("DataGraphsContainer");
            if (dataStyle != null) {
                styleSheets.Add(dataStyle);
                AddToClassList("custom-data-style");
            }

            if (dataUxml != null) {
                dataUxml.CloneTree(this);
            }
            else {
                Debug.LogError(
                    "Failed to load StyleSheet: BoxPlotStyle.uss.");
            }
        
        
        }
    }
}
