using Editor.Helpers;
using UnityEngine;
using UnityEngine.UIElements;


namespace Editor.Components.Graphs {
    public class CircleGraph : VisualElement{
        // Backing field for circle degree
        private float _circleDegree = 360f;
        private Label _titleLabel;

        /// <summary>
        /// The degree of the arc. 
        /// 0 means no arc drawn, 360 means full circle.
        /// </summary>
        public float CircleDegree
        {
            get => _circleDegree;
            set
            {
                // Clamp between 0 and 360
                _circleDegree = Mathf.Clamp(value, 0f, 360f);
                // Force a redraw
                MarkDirtyRepaint();
            }
        }

        // Optional: default constructor 
        public CircleGraph(string title = "1")
        {
            
            var circleGraphStyle = Resources.Load<StyleSheet>("Styles/CircleGraphStyle");
            if (circleGraphStyle != null) {
                styleSheets.Add(circleGraphStyle);
            }
            else {
                Debug.LogError(
                    "Failed to load StyleSheet: BoxPlotStyle.uss. Make sure it's placed in a Resources/Styles/ folder.");
            }
            
            _titleLabel = new Label(title).AddLabelClass("circle-graph-title-label");
            // We want to handle no mouse events, or you can change to PickingMode.Position if needed
            pickingMode = PickingMode.Ignore;

            // A default circle degree
            _circleDegree = 360f;
            generateVisualContent += GenerateVisualContent;
        }
        
        public string GetTitle() {
            return _titleLabel.text;
        }

        // The core method to draw your custom geometry with UI Toolkit
        private void GenerateVisualContent(MeshGenerationContext mgc)
        {
            var painter2D = mgc.painter2D;

            // 1) Determine the radius and center
            var width = contentRect.width;
            var height = contentRect.height;
            var radius = Mathf.Min(width, height) * 0.5f;
            var center = contentRect.center;

            // 2) Set up stroke & fill
            painter2D.lineWidth = 2f;
            painter2D.strokeColor = Color.white;
            // We won't fill the circle (transparent), but we can set fillColor anyway
            painter2D.fillColor = new Color(1f, 1f, 1f, 0f); // transparent

            // 3) Draw an arc from 0..CircleDegree
            var startAngle = 0f;
            // convert degrees to radians
            var endAngle = 2f * Mathf.PI * (_circleDegree / 360f);

            painter2D.BeginPath();
            painter2D.Arc(center, radius, startAngle, endAngle);
            painter2D.Stroke(); // stroke the arc with lineWidth=2, strokeColor=white
        }
    }
}