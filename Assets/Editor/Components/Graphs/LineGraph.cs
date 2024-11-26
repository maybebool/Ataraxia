using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Components.Graphs {
    public class LineGraph : VisualElement {
        // Private fields
        private List<float> _dataPoints = new();
        private int _maxDataPoints = 10;

        private VisualElement _chartContainer;
        private Label _titleLabel;

        // Constructor
        public LineGraph(string title = "Line Chart") {
            // Load the USS stylesheet
            var lineChartStyle = Resources.Load<StyleSheet>("Styles/LineGraphStyle");
            if (lineChartStyle != null) {
                styleSheets.Add(lineChartStyle);
            }
            else {
                Debug.LogError(
                    "Failed to load StyleSheet: LineChartStyle.uss. Make sure it's placed in a Resources/Styles/ folder.");
            }

            // Initialize the chart container
            _chartContainer = new VisualElement { name = "LineChartContainer" };
            _chartContainer.style.flexGrow = 1;
            _chartContainer.style.position = Position.Relative;

            // Title label
            _titleLabel = new Label(title) { name = "LineChartTitleLabel" };

            // Add title and container to the LineChart VisualElement
            Add(_titleLabel);
            Add(_chartContainer);

            // Apply the name to this VisualElement for styling
            name = "LineChart";
        }

        // Method to set the chart title
        public void SetTitle(string title) {
            _titleLabel.text = title;
        }

        // Method to add a data point
        public void AddDataPoint(float dataPoint) {
            _dataPoints.Add(dataPoint);

            // Ensure _maxDataPoints is greater than zero
            int maxPoints = Mathf.Max(_maxDataPoints, 1);

            while (_dataPoints.Count > maxPoints) {
                _dataPoints.RemoveAt(0);
            }
        }

        // Method to update the chart display
        public void UpdateChartDisplay() {
            if (_dataPoints == null || _dataPoints.Count < 2) return;
            if (_chartContainer == null) {
                Debug.LogError("LineChart visual elements are not properly initialized.");
                return;
            }

            // Force a layout pass to ensure the container has proper size values before calculating positions
            _chartContainer.MarkDirtyRepaint();
            _chartContainer.style.flexDirection = FlexDirection.Column;

            // Schedule the drawing code after the layout pass
            schedule.Execute(() => {
                // Ensure layout is ready
                float width = _chartContainer.layout.width;
                float height = _chartContainer.layout.height;

                if (width <= 0 || height <= 0) {
                    // Try again later if layout is not ready
                    schedule.Execute(UpdateChartDisplay);
                    return;
                }

                // Clear the chart container before drawing new elements
                _chartContainer.Clear();

                // Determine the minimum and maximum values for scaling
                float minValue = 0f; // Fixed to 0 as per requirements
                float maxValue = 10f; // Fixed to 10 as per requirements

                float valueRange = maxValue - minValue;
                if (Mathf.Approximately(valueRange, 0f)) {
                    valueRange = 1f; // Prevent division by zero
                }

                // Apply background colors based on value ranges
                ApplyBackgroundColors(width, height, minValue, maxValue, valueRange);

                // Draw grid lines and line segments
                DrawGridLines(width, height, minValue, maxValue, valueRange);
                DrawLineSegments(width, height, minValue, valueRange);

                // Force UI redraw
                MarkDirtyRepaint();
            });
        }

        private void ApplyBackgroundColors(float width, float height, float minValue, float maxValue,
            float valueRange) {
            // define the value thresholds for each color
            var thirdRange = valueRange / 3f;
            var greenThreshold = minValue + thirdRange;
            var yellowThreshold = minValue + 2f * thirdRange;

            // calculate positions for each color zone
            var greenHeight = ((greenThreshold - minValue) / valueRange) * height;
            var yellowHeight = ((yellowThreshold - greenThreshold) / valueRange) * height;
            var redHeight = height - greenHeight - yellowHeight;

            // Create and style the green background
            var greenZone = new VisualElement {
                name = "greenZone",
                style = {
                    left = 0,
                    top = height - greenHeight,
                    width = width,
                    height = greenHeight
                }
            };
            _chartContainer.Add(greenZone);

            // Create and style the yellow background
            var yellowZone = new VisualElement {
                name = "yellowZone",
                style = {
                    left = 0,
                    top = height - greenHeight - yellowHeight,
                    width = width,
                    height = yellowHeight
                }
            };
            _chartContainer.Add(yellowZone);

            // Create and style the red background
            var redZone = new VisualElement {
                name = "redZone",
                style = {
                    left = 0,
                    top = 0,
                    width = width,
                    height = redHeight
                }
            };
            _chartContainer.Add(redZone);
        }

        // Draws the line segments based on data points
        private void DrawLineSegments(float width, float height, float minValue, float valueRange) {
            for (int i = 1; i < _dataPoints.Count; i++) {
                var x1 = (i - 1) * (width / (_maxDataPoints - 1));
                var y1 = height - ((_dataPoints[i - 1] - minValue) / valueRange * height);
                var x2 = i * (width / (_maxDataPoints - 1));
                var y2 = height - ((_dataPoints[i] - minValue) / valueRange * height);

                // Length and angle of the line
                var length = Mathf.Sqrt(Mathf.Pow(x2 - x1, 2) + Mathf.Pow(y2 - y1, 2));
                var angle = Mathf.Atan2(y2 - y1, x2 - x1) * Mathf.Rad2Deg;

                // Create and style the line element
                var line = new VisualElement {
                    name = "lines",
                    style = {
                        left = x1,
                        top = y1,
                        width = length,
                        transformOrigin = new StyleTransformOrigin(new TransformOrigin(0, 0, 0)),
                        rotate = new StyleRotate(new Rotate(new Angle(angle, AngleUnit.Degree)))
                    }
                };

                // Add the line element to the chart container
                _chartContainer.Add(line);
            }
        }

        // Draws the grid lines and labels for the chart
        private void DrawGridLines(float width, float height, float minValue, float maxValue, float valueRange) {
            var stepX = width / (_maxDataPoints - 1);

            // Draw vertical grid lines
            for (int i = 0; i < _maxDataPoints; i++) {
                var columns = new VisualElement {
                    name = "columns",
                    style = {
                        left = i * stepX,
                        height = height,
                    }
                };
                _chartContainer.Add(columns);
            }

            // Draw horizontal grid lines and value labels
            var gridLines = 5; // Number of horizontal grid lines
            for (int i = 0; i <= gridLines; i++) {
                var value = minValue + i * (valueRange / gridLines);
                var yPos = height - (value - minValue) / valueRange * height;

                var rows = new VisualElement {
                    name = "rows",
                    style = {
                        top = yPos,
                        width = width,
                    }
                };
                _chartContainer.Add(rows);

                var valueLabel = new Label {
                    name = "valueLabel",
                    text = value.ToString("F1"),
                    style = {
                        top = yPos - 8,
                    }
                };
                _chartContainer.Add(valueLabel);
            }
        }
    }
}