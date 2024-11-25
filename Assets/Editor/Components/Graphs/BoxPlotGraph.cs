using ScriptableObjects;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Components.Graphs {
    public class BoxPlotGraph : VisualElement {
        private VisualElement _outerContainer;   // New: Top-level container holding labels and box plot side by side
        private VisualElement _labelsContainer;  // New: Container for the labels on the left
        private VisualElement _boxplotContainer;
        private VisualElement _minLine;
        private VisualElement _maxLine;
        private VisualElement _box;
        private VisualElement _medianLine;
        
        private VisualElement _upperWhiskerLine;
        private VisualElement _lowerWhiskerLine;

        // New: Labels for box plot statistics
        private Label _minLabel;
        private Label _q1Label;
        private Label _medianLabel;
        private Label _q3Label;
        private Label _maxLabel;

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

            // Initialize the outer container (row direction: labels on left, box plot on right)
            _outerContainer = new VisualElement { name = "BoxPlotOuterContainer" };
            _outerContainer.style.flexDirection = FlexDirection.Row;

            // Initialize the labels container (column direction to stack labels vertically)
            _labelsContainer = new VisualElement { name = "BoxPlotLabelsContainer" };
            _labelsContainer.style.flexDirection = FlexDirection.Column;
            _labelsContainer.style.justifyContent = Justify.SpaceBetween; 
            _labelsContainer.style.width = 60;  // Set a fixed width for labels container
            _labelsContainer.style.marginRight = 10;  // A bit of space between labels and the box plot

            // Create label elements for each box plot statistic
            _maxLabel = new Label { name = "BoxPlotMaxLabel" };
            _q3Label = new Label { name = "BoxPlotQ3Label" };
            _medianLabel = new Label { name = "BoxPlotMedianLabel" };
            _q1Label = new Label { name = "BoxPlotQ1Label" };
            _minLabel = new Label { name = "BoxPlotMinLabel" };

            // Add the labels to the labels container
            _labelsContainer.Add(_maxLabel);
            _labelsContainer.Add(_q3Label);
            _labelsContainer.Add(_medianLabel);
            _labelsContainer.Add(_q1Label);
            _labelsContainer.Add(_minLabel);

            // Initialize the box plot container
            _boxplotContainer = new VisualElement { name = "BoxPlotContainer" };
            _boxplotContainer.style.width = 100;
            _boxplotContainer.style.height = 200;
            _boxplotContainer.style.position = Position.Relative;
            _boxplotContainer.style.alignItems = Align.Center;

            // Create the line representing the minimum value
            _minLine = new VisualElement { name = "MinLine" };
            _minLine.style.position = Position.Absolute;
            _minLine.style.width = new Length(100, LengthUnit.Percent);
            _minLine.style.height = 2;

            // Create the line representing the maximum value
            _maxLine = new VisualElement { name = "MaxLine" };
            _maxLine.style.position = Position.Absolute;
            _maxLine.style.width = new Length(100, LengthUnit.Percent);
            _maxLine.style.height = 2;

            // Create the box representing the interquartile range (IQR)
            _box = new VisualElement { name = "Box" };
            _box.style.position = Position.Absolute;
            _box.style.width = new Length(60, LengthUnit.Percent);

            // Create the line representing the median value
            _medianLine = new VisualElement { name = "MedianLine" };
            _medianLine.style.position = Position.Absolute;
            _medianLine.style.width = new Length(60, LengthUnit.Percent);
            _medianLine.style.height = 2;
            
            // ** New: Create the upper whisker line
            _upperWhiskerLine = new VisualElement { name = "UpperWhiskerLine" };
            _upperWhiskerLine.style.position = Position.Absolute;
            _upperWhiskerLine.style.width = 2;

            // ** New: Create the lower whisker line
            _lowerWhiskerLine = new VisualElement { name = "LowerWhiskerLine" };
            _lowerWhiskerLine.style.position = Position.Absolute;
            _lowerWhiskerLine.style.width = 2;


            // Add these elements to the box plot container
            _boxplotContainer.Add(_minLine);
            _boxplotContainer.Add(_maxLine);
            _boxplotContainer.Add(_box);
            _boxplotContainer.Add(_medianLine);
            _boxplotContainer.Add(_upperWhiskerLine);  // Add upper whisker
            _boxplotContainer.Add(_lowerWhiskerLine);  // Add lower whisker

            // Add the labels container and box plot container to the outer container
            _outerContainer.Add(_labelsContainer);
            _outerContainer.Add(_boxplotContainer);

            // Add the outer container to the BoxPlotGraph VisualElement
            Add(_outerContainer);

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

            // Update the labels to display the calculated statistics
            _maxLabel.text = $"Max: {_boxPlotData.max:F2}";
            _q3Label.text = $"Q3: {_boxPlotData.q3:F2}";
            _medianLabel.text = $"Med: {_boxPlotData.median:F2}";
            _q1Label.text = $"Q1: {_boxPlotData.q1:F2}";
            _minLabel.text = $"Min: {_boxPlotData.min:F2}";

            // Force a layout pass to ensure the container has proper size values before calculating positions
            _boxplotContainer.MarkDirtyRepaint();
            _boxplotContainer.style.flexDirection = FlexDirection.Column;

            // Calculate layout after style changes
            this.schedule.Execute(() => {
                // Dimensions of the container
                float containerHeight = _boxplotContainer.layout.height > 0 ? _boxplotContainer.layout.height : 200f;

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

                
                float boxTop = containerHeight - q3Position;
                float boxBottom = containerHeight - q1Position;
                float boxHeight = boxBottom - boxTop;

                _box.style.top = boxTop;
                _box.style.height = boxHeight;
                // Update the positions of the lines and box
                // For a vertical boxplot:
                // - top coordinate of the container is 0 and the bottom is containerHeight
                _minLine.style.top = containerHeight - minPosition - 1;     // offset by half height
                _maxLine.style.top = containerHeight - maxPosition - 1;     // offset by half height
                _medianLine.style.top = containerHeight - medianPosition - 1;// offset by half height
                

                // Upper whisker from center of top of box to max line
                float upperWhiskerStartY = boxTop;
                float upperWhiskerEndY = containerHeight - maxPosition;
                float upperWhiskerHeight = upperWhiskerStartY - upperWhiskerEndY;

                _upperWhiskerLine.style.left = new Length(50, LengthUnit.Percent); // Center horizontally
                _upperWhiskerLine.style.marginLeft = -1; // Adjust for half the width of the line

                if (upperWhiskerHeight > 0) {
                    _upperWhiskerLine.style.top = upperWhiskerEndY;
                    _upperWhiskerLine.style.height = upperWhiskerHeight;
                    _upperWhiskerLine.style.display = DisplayStyle.Flex;
                } else {
                    _upperWhiskerLine.style.display = DisplayStyle.None;
                }

                // Lower whisker from center of bottom of box to min line
                float lowerWhiskerStartY = boxBottom;
                float lowerWhiskerEndY = containerHeight - minPosition;
                float lowerWhiskerHeight = lowerWhiskerEndY - lowerWhiskerStartY;

                _lowerWhiskerLine.style.left = new Length(50, LengthUnit.Percent); // Center horizontally
                _lowerWhiskerLine.style.marginLeft = -1; // Adjust for half the width of the line

                if (lowerWhiskerHeight > 0) {
                    _lowerWhiskerLine.style.top = lowerWhiskerStartY;
                    _lowerWhiskerLine.style.height = lowerWhiskerHeight;
                    _lowerWhiskerLine.style.display = DisplayStyle.Flex;
                } else {
                    _lowerWhiskerLine.style.display = DisplayStyle.None;
                }


                // Force UI redraw
                MarkDirtyRepaint();
            });
        }
    }
}