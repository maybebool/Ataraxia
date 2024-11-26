using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Components.Graphs
{
    public class LineGraph : VisualElement
    {
        // Private fields
        private List<float> _dataPoints = new();
        private float _elapsedTime = 0f;
        private int _maxDataPoints = 10;
        private float _updateInterval = 1f;
        private float _minValueAdjustment = 2f; // Adjustment value for min value
        private float _maxValueAdjustment = 2f; // Adjustment value for max value

        private VisualElement _chartContainer; // Container for the chart elements
        private Label _titleLabel;
        private bool _isScheduled = false;

        private Func<float> _getDataFunction; 
        private IVisualElementScheduledItem _updateScheduledItem;// Function to get data points

        // Public properties for external configuration
        public int MaxDataPoints
        {
            get => _maxDataPoints;
            set
            {
                if (value < 1)
                {
                    Debug.LogWarning("MaxDataPoints must be greater than zero. Setting to 1.");
                    _maxDataPoints = 1;
                }
                else
                {
                    _maxDataPoints = value;
                }
            }
        }


        public float MinValueAdjustment
        {
            get => _minValueAdjustment;
            set => _minValueAdjustment = value;
        }

        public float MaxValueAdjustment
        {
            get => _maxValueAdjustment;
            set => _maxValueAdjustment = value;
        }

        // Constructor
        public LineGraph(string title = "Line Chart")
        {
            // Load the USS stylesheet
            var lineChartStyle = Resources.Load<StyleSheet>("Styles/LineGraphStyle");
            if (lineChartStyle != null)
            {
                styleSheets.Add(lineChartStyle);
            }
            else
            {
                Debug.LogError("Failed to load StyleSheet: LineChartStyle.uss. Make sure it's placed in a Resources/Styles/ folder.");
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
        // public LineGraph(string title = "Line Chart", Func<float> getDataFunction = null, float updateInterval = 1f)
        // {
        //     _updateInterval = updateInterval;
        //
        //     // Load the USS stylesheet
        //     var lineChartStyle = Resources.Load<StyleSheet>("Styles/LineGraphStyle");
        //     if (lineChartStyle != null)
        //     {
        //         styleSheets.Add(lineChartStyle);
        //     }
        //     else
        //     {
        //         Debug.LogError("Failed to load StyleSheet: LineChartStyle.uss. Make sure it's placed in a Resources/Styles/ folder.");
        //     }
        //
        //     // Initialize the chart container
        //     _chartContainer = new VisualElement { name = "LineChartContainer" };
        //     _chartContainer.style.flexGrow = 1;
        //     _chartContainer.style.position = Position.Relative;
        //
        //     // Title label
        //     _titleLabel = new Label(title) { name = "LineChartTitleLabel" };
        //
        //     // Add title and container to the LineChart VisualElement
        //     Add(_titleLabel);
        //     Add(_chartContainer);
        //
        //     // Apply the name to this VisualElement for styling
        //     name = "LineChart";
        //
        //     // Set the data function and schedule updates
        //     if (getDataFunction != null)
        //     {
        //         SetDataFunction(getDataFunction);
        //     }
        // }

        // Method to set the data function
        // public void SetDataFunction(Func<float> getDataFunction)
        // {
        //     _getDataFunction = getDataFunction;
        //     if (_getDataFunction != null)
        //     {
        //         // Schedule UpdateChart if not already scheduled
        //         if (_updateScheduledItem == null)
        //         {
        //             _updateScheduledItem = schedule.Execute(UpdateChart).Every((long)(_updateInterval * 1000));
        //         }
        //     }
        //     else
        //     {
        //         // If data function is null, stop the scheduled item
        //         _updateScheduledItem?.Pause();
        //         _updateScheduledItem = null;
        //     }
        // }

        // Method to set data points
        // public void SetDataPoints(List<float> dataPoints)
        // {
        //     _dataPoints = dataPoints;
        //     UpdateChart();
        // }

        // Method to add a data point
        public void SetMaxDataPoints(int maxDataPoints)
        {
            _maxDataPoints = Mathf.Max(1, maxDataPoints);
        }

        // Method to add a data point
        public void AddDataPoint(float dataPoint)
        {
            _dataPoints.Add(dataPoint);

            // Ensure _maxDataPoints is greater than zero
            int maxPoints = Mathf.Max(_maxDataPoints, 1);

            while (_dataPoints.Count > maxPoints)
            {
                _dataPoints.RemoveAt(0);
            }
        }

        // Update the chart
        public void UpdateChart(float deltaTime)
        {
            _elapsedTime += deltaTime;

            // Ensure layout is ready
            if (resolvedStyle.width <= 0 || resolvedStyle.height <= 0)
            {
                // Wait for the next update
                return;
            }

            // Clear the chart container before drawing new elements
            _chartContainer.Clear();

            // Draw grid lines and line segments
            DrawGridLines();
            DrawLineSegments();
            MarkDirtyRepaint();
        }

        // Draws the line segments based on data points
        private void DrawLineSegments()
        {
            if (_dataPoints.Count < 2) return;

            float width = resolvedStyle.width;
            float height = resolvedStyle.height;

            // Determine the minimum and maximum values, adjusting them for better visualization
            float minValue = Mathf.Floor(Mathf.Min(_dataPoints.ToArray()) / 2) * 2 - _minValueAdjustment;
            float maxValue = Mathf.Ceil(Mathf.Max(_dataPoints.ToArray()) / 2) * 2 + _maxValueAdjustment;

            float valueRange = maxValue - minValue; // Calculate the range of the values

            for (int i = 1; i < _dataPoints.Count; i++)
            {
                // Calculate positions based on data points
                float x1 = (i - 1) * (width / (_maxDataPoints - 1));
                float y1 = height - ((_dataPoints[i - 1] - minValue) / valueRange * height);
                float x2 = i * (width / (_maxDataPoints - 1));
                float y2 = height - ((_dataPoints[i] - minValue) / valueRange * height);

                // Length and angle of the line
                float length = Mathf.Sqrt(Mathf.Pow(x2 - x1, 2) + Mathf.Pow(y2 - y1, 2));
                float angle = Mathf.Atan2(y2 - y1, x2 - x1) * Mathf.Rad2Deg;

                // Create and style the line element
                VisualElement line = new VisualElement();
                line.style.position = Position.Absolute;
                line.style.left = x1;
                line.style.top = y1;
                line.style.width = length;
                line.style.height = 2;
                line.style.backgroundColor = new Color(0, 0, 0);
                line.style.transformOrigin = new StyleTransformOrigin(new TransformOrigin(0, 0, 0));
                line.style.rotate = new StyleRotate(new Rotate(new Angle(angle, AngleUnit.Degree)));

                // Add the line element to the chart container
                _chartContainer.Add(line);
            }
        }

        // Draws the grid lines and labels for the chart
        private void DrawGridLines()
        {
            float width = resolvedStyle.width;
            float height = resolvedStyle.height;

            float stepX = width / (_maxDataPoints - 1);

            // Determine the minimum and maximum values, adjusting them for better visualization
            float minValue = Mathf.Floor(Mathf.Min(_dataPoints.ToArray()) / 2) * 2 - _minValueAdjustment;
            float maxValue = Mathf.Ceil(Mathf.Max(_dataPoints.ToArray()) / 2) * 2 + _maxValueAdjustment;

            float valueRange = maxValue - minValue; // Calculate the range of the values

            // Draw vertical grid lines and time labels
            for (int i = 0; i < _maxDataPoints; i++)
            {
                VisualElement verticalLine = new VisualElement();
                verticalLine.style.position = Position.Absolute;
                verticalLine.style.left = i * stepX;
                verticalLine.style.top = 0;
                verticalLine.style.width = (i % 5 == 0) ? 2 : 1; // Thicker line every 5 steps
                verticalLine.style.height = height;
                verticalLine.style.backgroundColor = new Color(0.7f, 0.7f, 0.7f);

                _chartContainer.Add(verticalLine);

                // Only display labels every 5 intervals
                if (i % 5 == 0)
                {
                    Label timeLabel = new Label();
                    float totalTime = _elapsedTime - (_maxDataPoints - 1 - i) * _updateInterval;

                    timeLabel.text = FormatTime(totalTime);
                    timeLabel.style.position = Position.Absolute;
                    timeLabel.style.left = i * stepX - 15;
                    timeLabel.style.top = height + 2;
                    timeLabel.style.fontSize = 12;
                    timeLabel.style.color = new Color(0, 0, 0);

                    _chartContainer.Add(timeLabel);
                }
            }

            // Draw horizontal grid lines and value labels
            for (float value = minValue; value <= maxValue; value += 2)
            {
                // Calculate y position based on the value
                float yPos = height - ((value - minValue) / valueRange * height);
                VisualElement horizontalLine = new VisualElement();
                horizontalLine.style.position = Position.Absolute;
                horizontalLine.style.left = 0;
                horizontalLine.style.top = yPos;
                horizontalLine.style.width = width;
                horizontalLine.style.height = 1;
                horizontalLine.style.backgroundColor = new Color(0.7f, 0.7f, 0.7f);

                _chartContainer.Add(horizontalLine);

                Label valueLabel = new Label();
                valueLabel.text = value.ToString("F1");
                valueLabel.style.position = Position.Absolute;
                valueLabel.style.left = -40;
                valueLabel.style.top = yPos - 8;
                valueLabel.style.fontSize = 12;
                valueLabel.style.color = new Color(0, 0, 0);

                _chartContainer.Add(valueLabel);
            }
        }

        // Formats time in MM:SS format
        private string FormatTime(float timeInSeconds)
        {
            int minutes = Mathf.FloorToInt(timeInSeconds / 60F);
            int seconds = Mathf.FloorToInt(timeInSeconds % 60F);
            return string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}