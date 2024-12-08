using ScriptableObjects;
using UnityEngine;
using UnityEngine.UIElements;
using Editor.Helpers;

namespace Editor.Components.Graphs {
    public class BoxPlotGraph : VisualElement {
        private Label _titleLabel;
        private readonly VisualElement _boxplotContainer;
        private readonly VisualElement _minLine;
        private readonly VisualElement _maxLine;
        private readonly VisualElement _box;
        private readonly VisualElement _medianLine;
        private readonly VisualElement _upperWhiskerLine;
        private readonly VisualElement _lowerWhiskerLine;
        
        private readonly Label _minLabel;
        private readonly Label _q1Label;
        private readonly Label _medianLabel;
        private readonly Label _q3Label;
        private readonly Label _maxLabel;
        private DataContainer _boxPlotData;
        
        private float _minValue;
        private float _q1Value;
        private float _medianValue;
        private float _q3Value;
        private float _maxValue;

        public BoxPlotGraph(string title = "1") {
            // Load the USS stylesheet
            var boxPlotStyle = Resources.Load<StyleSheet>("Styles/BoxPlotStyle");
            if (boxPlotStyle != null) {
                styleSheets.Add(boxPlotStyle);
            }
            else {
                Debug.LogError(
                    "Failed to load StyleSheet: BoxPlotStyle.uss. Make sure it's placed in a Resources/Styles/ folder.");
            }

            this.AddClass("boxPlot");
            var mainContainer = new VisualElement().AddClass("boxPlotMainContainer");
            _titleLabel = new Label(title).AddLabelClass("boxPlotTitleLabel");
            var outerContainer = new VisualElement().AddClass("boxPlotOuterContainer");
            var labelsContainer = new VisualElement().AddClass("boxPlotLabelsContainer");
            
            _maxLabel = new Label().AddLabelClass("boxPlotMaxLabel");
            _q3Label = new Label().AddLabelClass("boxPlotQ3Label");
            _medianLabel = new Label().AddLabelClass("boxPlotMedianLabel");
            _q1Label = new Label().AddLabelClass("boxPlotQ1Label");
            _minLabel = new Label().AddLabelClass("boxPlotMinLabel");
            
            _boxplotContainer = new VisualElement().AddClass("boxPlotContainer");
            _minLine = new VisualElement().AddClass("minLine");
            _maxLine = new VisualElement().AddClass("maxLine");
            _box = new VisualElement().AddClass("box");
            _medianLine = new VisualElement().AddClass("medianLine");
            _upperWhiskerLine = new VisualElement().AddClass("upperWhiskerLine");
            _lowerWhiskerLine = new VisualElement().AddClass("lowerWhiskerLine");
            
            labelsContainer.Add(_maxLabel);
            labelsContainer.Add(_q3Label);
            labelsContainer.Add(_medianLabel);
            labelsContainer.Add(_q1Label);
            labelsContainer.Add(_minLabel);
            
            _boxplotContainer.Add(_minLine);
            _boxplotContainer.Add(_maxLine);
            _boxplotContainer.Add(_box);
            _boxplotContainer.Add(_medianLine);
            _boxplotContainer.Add(_upperWhiskerLine);
            _boxplotContainer.Add(_lowerWhiskerLine);
            
            outerContainer.Add(labelsContainer);
            outerContainer.Add(_boxplotContainer);
            
            
            mainContainer.Add(_titleLabel);
            mainContainer.Add(outerContainer);
            Add(mainContainer);
        }

        public void SetBoxPlotData(DataContainer data) {
            _boxPlotData = data;
            UpdateBoxPlotDisplay();
            
        }
        
        public void SetTitle(string title) {
            _titleLabel.text = title;
        }

        private void UpdateBoxPlotDisplay() {
            if (_boxPlotData == null || _boxPlotData.tremorValues == null || _boxPlotData.tremorValues.Count == 0) return;
            if (_boxplotContainer == null || _minLine == null || _maxLine == null || _box == null ||
                _medianLine == null) {
                Debug.LogError("BoxPlotGraph visual elements are not properly initialized.");
                return;
            }
            
            _maxLabel.text = $"Max: {_boxPlotData.max:F2}";
            _q3Label.text = $"Q3: {_boxPlotData.q3:F2}";
            _medianLabel.text = $"Med: {_boxPlotData.median:F2}";
            _q1Label.text = $"Q1: {_boxPlotData.q1:F2}";
            _minLabel.text = $"Min: {_boxPlotData.min:F2}";
            
            _boxplotContainer.MarkDirtyRepaint();
            
            schedule.Execute(() => {
                var containerHeight = _boxplotContainer.layout.height > 0 ? _boxplotContainer.layout.height : 200f;
                var minValue = _boxPlotData.min;
                var maxValue = _boxPlotData.max;
                var range = maxValue - minValue;

                if (Mathf.Approximately(range, 0f)) {
                    // In case all values are the same
                    range = 1f;
                }

                var minPosition = containerHeight * (_boxPlotData.min - minValue) / range;
                var maxPosition = containerHeight * (_boxPlotData.max - minValue) / range;
                var q1Position = containerHeight * (_boxPlotData.q1 - minValue) / range;
                var medianPosition = containerHeight * (_boxPlotData.median - minValue) / range;
                var q3Position = containerHeight * (_boxPlotData.q3 - minValue) / range;
                
                var boxTop = containerHeight - q3Position;
                var boxBottom = containerHeight - q1Position;
                var boxHeight = boxBottom - boxTop;

                _box.style.top = boxTop;
                _box.style.height = boxHeight;
                _minLine.style.top = containerHeight - minPosition - 1;
                _maxLine.style.top = containerHeight - maxPosition - 1;
                _medianLine.style.top = containerHeight - medianPosition - 1;
                
                var upperWhiskerStartY = boxTop;
                var upperWhiskerEndY = containerHeight - maxPosition;
                var upperWhiskerHeight = upperWhiskerStartY - upperWhiskerEndY;

                _upperWhiskerLine.style.left = new Length(50, LengthUnit.Percent);
                _upperWhiskerLine.style.marginLeft = -1;

                if (upperWhiskerHeight > 0) {
                    _upperWhiskerLine.style.top = upperWhiskerEndY;
                    _upperWhiskerLine.style.height = upperWhiskerHeight;
                    _upperWhiskerLine.style.display = DisplayStyle.Flex;
                }
                else {
                    _upperWhiskerLine.style.display = DisplayStyle.None;
                }
                
                var lowerWhiskerStartY = boxBottom;
                var lowerWhiskerEndY = containerHeight - minPosition;
                var lowerWhiskerHeight = lowerWhiskerEndY - lowerWhiskerStartY;

                _lowerWhiskerLine.style.left = new Length(50, LengthUnit.Percent);
                _lowerWhiskerLine.style.marginLeft = -1; 

                if (lowerWhiskerHeight > 0) {
                    _lowerWhiskerLine.style.top = lowerWhiskerStartY;
                    _lowerWhiskerLine.style.height = lowerWhiskerHeight;
                    _lowerWhiskerLine.style.display = DisplayStyle.Flex;
                }
                else {
                    _lowerWhiskerLine.style.display = DisplayStyle.None;
                }
                
                MarkDirtyRepaint();
            });
        }
        
        public string GetTitle() {
            return _titleLabel.text;
        }
    }
}