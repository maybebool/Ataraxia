using ScriptableObjects;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Components.Graphs {
    public class BoxPlotGraph : VisualElement {
        private VisualElement _boxplotContainer;
        private VisualElement _minLine;
        private VisualElement _maxLine;
        private VisualElement _box;
        private VisualElement _medianLine;

        private BoxPlotData _boxPlotData;

        public BoxPlotGraph() {
            // Load the USS stylesheet
            var boxPlotStyle = Resources.Load<StyleSheet>("Styles/BoxPlotStyle");
            if (boxPlotStyle != null) {
                styleSheets.Add(boxPlotStyle);
            }
            else {
                Debug.LogError("Failed to load StyleSheet: BoxPlotStyle.uss. Make sure it's placed in a Resources/Styles/ folder.");
            }

            // Initialize the box plot container
            _boxplotContainer = new VisualElement { name = "BoxPlotContainer" };
            // Apply initial styling
            _boxplotContainer.style.width = 200;
            _boxplotContainer.style.height = 100;
            _boxplotContainer.style.position = Position.Relative;
            _boxplotContainer.style.alignItems = Align.Center;

            // Create the line representing the minimum value
            _minLine = new VisualElement {
                name = "MinLine",
                style = {
                    position = Position.Absolute,
                    width = new Length(100, LengthUnit.Percent),
                    height = 2,
                    
                }
            };

            // Create the line representing the maximum value
            _maxLine = new VisualElement {
                name = "MaxLine",
                style = {
                    position = Position.Absolute,
                    width = new Length(100, LengthUnit.Percent),
                    height = 2,
                    
                }
            };

            // Create the box representing the interquartile range (IQR)
            _box = new VisualElement {
                name = "Box",
                style = {
                    position = Position.Absolute,
                    width = new Length(100, LengthUnit.Percent),
                }
            };

            // Create the line representing the median value
            _medianLine = new VisualElement {
                name = "MedianLine",
                style = {
                    position = Position.Absolute,
                    width = new Length(100, LengthUnit.Percent),
                    height = 2,
                    
                }
            };

            // Add these elements to the container
            _boxplotContainer.Add(_minLine);
            _boxplotContainer.Add(_maxLine);
            _boxplotContainer.Add(_box);
            _boxplotContainer.Add(_medianLine);

            // Add the container to the BoxPlotGraph VisualElement
            Add(_boxplotContainer);

            // Apply the name to this VisualElement for styling
            name = "BoxPlot";
        }

        public void SetBoxPlotData(BoxPlotData data) {
            _boxPlotData = data;
            UpdateBoxPlotDisplay();
        }

        private void UpdateBoxPlotDisplay() {
            if (_boxPlotData == null || _boxPlotData.values == null || _boxPlotData.values.Length == 0) return;
            if (_boxplotContainer == null || _minLine == null || _maxLine == null || _box == null || _medianLine == null) {
                Debug.LogError("BoxPlotGraph visual elements are not properly initialized.");
                return;
            }

            // Force a layout pass to ensure the container has proper size values before calculating positions
            _boxplotContainer.MarkDirtyRepaint();
            _boxplotContainer.style.flexDirection = FlexDirection.Column;

            // Calculate layout after style changes
            this.schedule.Execute(() => {
                // Dimensions of the container
                float containerHeight = _boxplotContainer.layout.height > 0 ? _boxplotContainer.layout.height : 200f;
                float containerWidth = _boxplotContainer.layout.width > 0 ? _boxplotContainer.layout.width : 200f;

                // Normalizing values based on min and max
                float minValue = _boxPlotData.min;
                float maxValue = _boxPlotData.max;
                float range = maxValue - minValue;

                if (Mathf.Approximately(range, 0f)) {
                    range = 1f; // In case all values are the same
                }

                // Calculate vertical positions (assuming vertical boxplot)
                float minPosition = containerHeight * (_boxPlotData.min - minValue) / range;
                float maxPosition = containerHeight * (_boxPlotData.max - minValue) / range;
                float q1Position = containerHeight * (_boxPlotData.q1 - minValue) / range;
                float medianPosition = containerHeight * (_boxPlotData.median - minValue) / range;
                float q3Position = containerHeight * (_boxPlotData.q3 - minValue) / range;

                // Update the positions of the lines and box
                // For a vertical boxplot:
                // - top coordinate of the container is 0 and the bottom is containerHeight
                _minLine.style.top = containerHeight - minPosition - 1;     // offset by half height
                _maxLine.style.top = containerHeight - maxPosition - 1;     // offset by half height
                _medianLine.style.top = containerHeight - medianPosition - 1;// offset by half height

                // The box spans from q1 to q3
                float boxHeight = q3Position - q1Position;
                _box.style.top = containerHeight - q3Position;
                _box.style.height = boxHeight;

                // Force UI redraw
                MarkDirtyRepaint();
            });
        }
    }
}