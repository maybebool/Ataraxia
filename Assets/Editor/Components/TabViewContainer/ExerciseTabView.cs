using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Components.TabViewContainer {
    public class ExerciseTabView : VisualElement {
    
        public ExerciseTabView() {
            var exerciseTabViewStyle = Resources.Load<StyleSheet>("Styles/ExerciseTabStyle");
            var exerciseUxml = Resources.Load<VisualTreeAsset>("ExerciseTabContainer");
            if (exerciseTabViewStyle != null) {
                styleSheets.Add(exerciseTabViewStyle);
                AddToClassList("custom-exercise-style");
            }

            if (exerciseUxml != null) {
                exerciseUxml.CloneTree(this);
            }
            else {
                Debug.LogError(
                    "Failed to load StyleSheet: BoxPlotStyle.uss.");
            }
        }
    }
}