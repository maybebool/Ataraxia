using Editor.Helpers;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.UIElements;


namespace Editor.Components.Graphs {
    public class CircleGraph : VisualElement {
        private DataContainer _scO;
        private float _circleDegree;
        private float _circlePercentage = 100f;
        private readonly VisualElement _mainCircleContainer;
        private Label _percentageLabel;


        /// <summary>
        /// Represents a graphical component for displaying circular graphs in the Unity Editor interface.
        /// </summary>
        /// <remarks>
        /// The CircleGraph class is designed to render a circular progress graph with a customizable title and subtitle.
        /// It supports updating the circle's fill percentage dynamically and displays the percentage
        /// visually in the center of the graph.
        /// This class derives from the VisualElement class and is styled using an external StyleSheet resource.
        /// </remarks>
        public CircleGraph(string title = "1", string subtitle = "2") {
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
            var subtitleLabel = new Label(subtitle);
            titleLabel.AddToClassList("title-label");
            subtitleLabel.AddToClassList("subtitle-label");
            _mainCircleContainer = new VisualElement().AddClass("main-circle-container");
            _mainCircleContainer.generateVisualContent += OnGenerateCircleArc;
            _percentageLabel = new Label();
            _percentageLabel.style.unityTextAlign = TextAnchor.MiddleCenter;
            _percentageLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
            _mainCircleContainer.Add(_percentageLabel);
            Add(titleLabel);
            Add(subtitleLabel);
            Add(_mainCircleContainer);

            _circlePercentage = 100f;
            UpdatePercentageLabel();
        }

        public void SetCirclePercentage(float newPercentage) {
            _circlePercentage = Mathf.Clamp(newPercentage, 0f, 100f);
            UpdatePercentageLabel();
            _mainCircleContainer.MarkDirtyRepaint();
        }

        private void UpdatePercentageLabel() {
            var percent = Mathf.RoundToInt(_circlePercentage);
            _percentageLabel.text = percent + "%";
        }

        public void UpdateCircleThresholds(
            float targetObjectOuterHeightThresholdTop,
            float targetObjectOuterHeightThresholdFloor,
            float targetObjectInnerHeightThresholdTop,
            float targetObjectInnerHeightThresholdFloor,
            float playerHeight
        ) {
            if (playerHeight > targetObjectOuterHeightThresholdTop 
                || playerHeight < targetObjectOuterHeightThresholdFloor) {
                _circlePercentage -= 2f;
            }
            else {
                if (playerHeight >= targetObjectInnerHeightThresholdFloor 
                    && playerHeight <= targetObjectInnerHeightThresholdTop) {
                    _circlePercentage += 1f;
                }
            }
            _circlePercentage = Mathf.Clamp(_circlePercentage, 0f, 100f);

            UpdatePercentageLabel();
            _mainCircleContainer.MarkDirtyRepaint();
        }


        /// <summary>
        /// Generates a visual representation of a circular arc inside the circle graph using a specified percentage value.
        /// </summary>
        /// <param name="mgc">The mesh generation context used to draw the circular arc.</param>
        /// <remarks>
        /// This method is invoked as part of the visual content generation process. It calculates and draws the
        /// arc segment of the circle graph based on the current percentage value using the painter2D object.
        /// The arc's parameters, such as radius, start angle, and end angle, are determined dynamically based on
        /// the circle's dimensions and percentage value.
        /// </remarks>
        private void OnGenerateCircleArc(MeshGenerationContext mgc) {
            var painter2D = mgc.painter2D;
            painter2D.lineWidth = 2f;
            painter2D.strokeColor = Color.white;
            painter2D.fillColor = Color.clear;

            var width = _mainCircleContainer.contentRect.width;
            var height = _mainCircleContainer.contentRect.height;

            var radius = Mathf.Min(width, height) * 0.5f;
            var center = _mainCircleContainer.contentRect.center;
            var startAngleDeg = -90f;
            var endAngleDeg = startAngleDeg + (_circlePercentage / 100f) * 360f;

            DrawArcManually(painter2D, center, radius, startAngleDeg, endAngleDeg);
        }


        /// <summary>
        /// Draws an arc on the given <c>Painter2D</c> canvas based on specified parameters including center,
        /// radius, start angle, and end angle.
        /// </summary>
        /// <param name="painter2D">
        /// The <c>Painter2D</c> object used to render the arc. It is responsible for managing the drawing context.
        /// </param>
        /// <param name="center">
        /// The center position of the arc in the coordinate system of the parent container.
        /// </param>
        /// <param name="radius">
        /// The radius of the arc, determining its size.
        /// </param>
        /// <param name="startAngleDeg">
        /// The starting angle of the arc in degrees, measured clockwise from the upward vertical direction.
        /// </param>
        /// <param name="endAngleDeg">
        /// The ending angle of the arc in degrees, measured clockwise from the upward vertical direction.
        /// </param>
        private void DrawArcManually(Painter2D painter2D, Vector2 center,
            float radius, float startAngleDeg, float endAngleDeg) {
            
            var startAngleRad = Mathf.Deg2Rad * startAngleDeg;
            var endAngleRad = Mathf.Deg2Rad * endAngleDeg;
            
            var segments = 64;
            var angleStep = (endAngleRad - startAngleRad) / segments;

            painter2D.BeginPath();
            
            var angle = startAngleRad;
            var x0 = center.x + radius * Mathf.Cos(angle);
            var y0 = center.y + radius * Mathf.Sin(angle);
            painter2D.MoveTo(new Vector2(x0, y0));

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