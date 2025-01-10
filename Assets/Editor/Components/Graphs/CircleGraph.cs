using Editor.Helpers;
using UnityEngine;
using UnityEngine.UIElements;


namespace Editor.Components.Graphs {
    public class CircleGraph : VisualElement {
        private float _circleDegree;
        private float _circlePercentage = 100f;
        private readonly VisualElement _mainCircleContainer;
        private Label _percentageLabel;


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
            _percentageLabel = new Label();
            _percentageLabel.style.unityTextAlign = TextAnchor.MiddleCenter;
            _percentageLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
            _mainCircleContainer.Add(_percentageLabel);
            Add(titleLabel);
            Add(_mainCircleContainer);

            _circlePercentage = 100f;
            UpdatePercentageLabel();
        }

        public void SetCirclePercentage(float newPercentage) {
            _circlePercentage = Mathf.Clamp(newPercentage, 0f, 100f);
            UpdatePercentageLabel();
            // Force a redraw
            _mainCircleContainer.MarkDirtyRepaint();
        }

        private void UpdatePercentageLabel() {
            // Convert circleDegree to a percentage
            var percent = Mathf.RoundToInt((_circleDegree / 360f) * 100f);
            _percentageLabel.text = percent + "%";
        }

        public void UpdateCircleThresholds(
            float targetObjectOuterHeightThresholdTop,
            float targetObjectOuterHeightThresholdFloor,
            float targetObjectInnerHeightThresholdTop,
            float targetObjectInnerHeightThresholdFloor,
            float playerHeight
        )
        {
            // Example from your logic:
            // If playerHeight is outside outer thresholds -> subtract 2%
            if (playerHeight > targetObjectOuterHeightThresholdTop 
                || playerHeight < targetObjectOuterHeightThresholdFloor)
            {
                _circlePercentage -= 2f;
            }
            else
            {
                // If it's in the 'middle ring', do nothing,
                // but if it's within inner thresholds, add 1%.
                if (playerHeight >= targetObjectInnerHeightThresholdFloor 
                    && playerHeight <= targetObjectInnerHeightThresholdTop)
                {
                    // Add 1% up to 100
                    _circlePercentage += 1f;
                }
            }

            // Clamp 0..100
            _circlePercentage = Mathf.Clamp(_circlePercentage, 0f, 100f);

            // Update label and redraw
            UpdatePercentageLabel();
            _mainCircleContainer.MarkDirtyRepaint();
        }
        

        private void OnGenerateCircleArc(MeshGenerationContext mgc) {
            var painter2D = mgc.painter2D;
            painter2D.lineWidth = 2f;
            painter2D.strokeColor = Color.white;
            painter2D.fillColor = Color.clear;

            var width = _mainCircleContainer.contentRect.width;
            var height = _mainCircleContainer.contentRect.height;

            // Fit the arc in the container
            var radius = Mathf.Min(width, height) * 0.5f;
            var center = _mainCircleContainer.contentRect.center;

            // Start at top (12 o'clock) => -90 deg,
            // sweep clockwise by (_circlePercentage / 100f * 360f)
            var startAngleDeg = -90f;
            var endAngleDeg = startAngleDeg + (_circlePercentage / 100f) * 360f;

            DrawArcManually(painter2D, center, radius, startAngleDeg, endAngleDeg);
        }


        private void DrawArcManually(
            Painter2D painter2D,
            Vector2 center,
            float radius,
            float startAngleDeg,
            float endAngleDeg
        )
        {
            // Convert degrees to radians
            var startAngleRad = Mathf.Deg2Rad * startAngleDeg;
            var endAngleRad   = Mathf.Deg2Rad * endAngleDeg;

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
            for (int i = 1; i <= segments; i++)
            {
                angle = startAngleRad + angleStep * i;
                var x = center.x + radius * Mathf.Cos(angle);
                var y = center.y + radius * Mathf.Sin(angle);
                painter2D.LineTo(new Vector2(x, y));
            }

            painter2D.Stroke();
        }
    }
}