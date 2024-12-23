using System.Collections.Generic;
using Editor.Helpers;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Components.Graphs {
    [UxmlElement("LineGraph")]
    public partial class LineGraph : VisualElement {
        public List<float> DataPoints => _dataPoints;
        private List<float> _dataPoints = new();
        private int _maxDataPoints = 10;
        private VisualElement _chartContainer;
        private Label _titleLabel;
        private float minValue = 0f; 
        private float maxValue = 10f;
        private float valueRange;
        
        public LineGraph() {
            
        }
        
        public LineGraph(string title = "Line Chart") {
            var lineChartStyle = Resources.Load<StyleSheet>("Styles/LineGraphStyle");
            if (lineChartStyle != null) {
                styleSheets.Add(lineChartStyle);
            }
            else {
                Debug.LogError(
                    "Failed to load StyleSheet: LineChartStyle.uss. Make sure it's placed in a Resources/Styles/ folder.");
            }
            
            _titleLabel = new Label(title).AddLabelClass("lineGraphTitleLabel");
            _chartContainer = new VisualElement().AddClass("lineGraphContainer");
            this.AddClass("lineGraph");
            
            Add(_titleLabel);
            Add(_chartContainer);
        }
        
        public void AddDataPoint(float dataPoint) {
            _dataPoints.Add(dataPoint);
            var maxPoints = Mathf.Max(_maxDataPoints, 1);

            while (_dataPoints.Count > maxPoints) {
                _dataPoints.RemoveAt(0);
            }
            
            // Debug.Log($"Current DataPoints Count: {_dataPoints.Count}");
            // Debug.Log(" Time " + Time.time);
            // Debug.Log($"DataPoints Values: {string.Join(", ", _dataPoints)}");
        }
        
        public void UpdateChartDisplay() {
            if (_dataPoints == null || _dataPoints.Count < 2) return;
            if (_chartContainer == null) {
                Debug.LogError("LineChart visual elements are not properly initialized.");
                return;
            }
            
            _chartContainer.MarkDirtyRepaint();
            _chartContainer.style.flexDirection = FlexDirection.Column;
            
            schedule.Execute(() => {
                var width = _chartContainer.layout.width;
                var height = _chartContainer.layout.height;

                if (width <= 0 || height <= 0) {
                    schedule.Execute(UpdateChartDisplay);
                    return;
                }
                
                _chartContainer.Clear();

                valueRange = maxValue - minValue;
                // Prevent division by zero
                if (Mathf.Approximately(valueRange, 0f)) {
                    valueRange = 1f; 
                }
                
                ApplyBackgroundColors(width, height, minValue, valueRange);
                DrawGridLines(width, height, minValue, valueRange);
                DrawLineSegments(width, height, minValue, valueRange);
                MarkDirtyRepaint();
            });
        }

        private void ApplyBackgroundColors(float width, float height, float minValue,
            float valueRange) {
            
            var thirdRange = valueRange / 3f;
            var greenThreshold = minValue + thirdRange;
            var yellowThreshold = minValue + 2f * thirdRange;
            var greenHeight = ((greenThreshold - minValue) / valueRange) * height;
            var yellowHeight = ((yellowThreshold - greenThreshold) / valueRange) * height;
            var redHeight = height - greenHeight - yellowHeight;
            
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
        
        private void DrawLineSegments(float width, float height, float minValue, float valueRange) {
            for (int i = 1; i < _dataPoints.Count; i++) {
                var x1 = (i - 1) * (width / (_maxDataPoints - 1));
                var y1 = height - ((_dataPoints[i - 1] - minValue) / valueRange * height);
                var x2 = i * (width / (_maxDataPoints - 1));
                var y2 = height - ((_dataPoints[i] - minValue) / valueRange * height);
                var length = Mathf.Sqrt(Mathf.Pow(x2 - x1, 2) + Mathf.Pow(y2 - y1, 2));
                var angle = Mathf.Atan2(y2 - y1, x2 - x1) * Mathf.Rad2Deg;
                
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
                
                _chartContainer.Add(line);
            }
        }
        
        private void DrawGridLines(float width, float height, float minValue, float valueRange) {
            var stepX = width / (_maxDataPoints - 1);
            
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
            
            var gridLines = 5; 
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
        
        public void ClearData() {
            _dataPoints.Clear();
            UpdateChartDisplay();
        }
    }
}