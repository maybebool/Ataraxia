using Editor.Helpers;
using UnityEngine;
using UnityEngine.UIElements;


namespace Editor.Components.Graphs {
    public class CircleGraph : VisualElement {
        private float _circleDegree;
        private readonly VisualElement _mainCircleContainer;

        public float CircleDegree {
            get => _circleDegree;
            set {
                _circleDegree = Mathf.Clamp(value, 0f, 360f);
                _mainCircleContainer.MarkDirtyRepaint();
            }
        }


        public CircleGraph(string title = "1") {
            // Load the style
            var circleGraphStyle = Resources.Load<StyleSheet>("Styles/CircleGraphStyle");
            if (circleGraphStyle != null) {
                styleSheets.Add(circleGraphStyle);
            }
            else {
                Debug.LogError("Failed to load StyleSheet: CircleGraphStyle.uss. Check the Resources/Styles/ folder.");
            }

            style.overflow = Overflow.Visible;

            AddToClassList("circle-graph");
            pickingMode = PickingMode.Ignore;

            var titleLabel = new Label(title);
            _mainCircleContainer = new VisualElement().AddClass("main-circle-container");
            _mainCircleContainer.generateVisualContent += OnGenerateCircleArc;
            Add(titleLabel);
            Add(_mainCircleContainer);
        }

        private void OnGenerateCircleArc(MeshGenerationContext mgc) {
            var painter2D = mgc.painter2D;
            painter2D.lineWidth = 2f;
            painter2D.strokeColor = Color.white;
            painter2D.fillColor = Color.clear;

            var width = 100;
            var height = 100;
            var radius = Mathf.Min(width, height) * 0.5f;
            var center = _mainCircleContainer.contentRect.center;

            float startAngleDeg = -90f;
            float endAngleDeg   = startAngleDeg + _circleDegree; // sweep clockwise by _circleDegree
            // Draw a full 360-degree circle manually:
            DrawArcManually(painter2D, center, radius, startAngleDeg, endAngleDeg);
        }

        private void DrawArcManually(
            Painter2D painter2D,
            Vector2 center, 
            float radius, 
            float startAngleDeg,
            float endAngleDeg) 
        {
            // Convert degrees to radians
            var startAngleRad = Mathf.Deg2Rad * startAngleDeg;
            var endAngleRad = Mathf.Deg2Rad * endAngleDeg;

            // Decide how many segments to use (higher = smoother circle)
            var segments = 64;
            var angleStep = (endAngleRad - startAngleRad) / segments;

            painter2D.BeginPath();

            // Move to first point
            var angle = startAngleRad;
            var x0 = center.x + radius * Mathf.Cos(angle);
            var y0 = center.y + radius * Mathf.Sin(angle);
            painter2D.MoveTo(new Vector2(x0, y0));

            // LineTo each subsequent segment
            for (int i = 1; i <= segments; i++) {
                angle = startAngleRad + angleStep * i;
                var x = center.x + radius * Mathf.Cos(angle);
                var y = center.y + radius * Mathf.Sin(angle);
                painter2D.LineTo(new Vector2(x, y));
            }

            painter2D.Stroke();
        }
    }
}