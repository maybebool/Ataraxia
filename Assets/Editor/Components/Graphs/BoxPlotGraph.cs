using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Editor.Helpers;
using ScriptableObjects;

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
        
        private List<float> _dataPoints = new();
        
        private float _min;
        private float _max;
        private float _median;
        private float _q1;
        private float _q3;

        private List<float> MinValues { get; } = new();
        private List<float> Q1Values { get; } = new();
        private List<float> MedianValues { get; } = new();
        private List<float> Q3Values { get; } = new();
        private List<float> MaxValues { get; } = new();

        public IReadOnlyList<float> GetMinValues() => MinValues;
        public IReadOnlyList<float> GetQ1Values() => Q1Values;
        public IReadOnlyList<float> GetMedianValues() => MedianValues;
        public IReadOnlyList<float> GetQ3Values() => Q3Values;
        public IReadOnlyList<float> GetMaxValues() => MaxValues;

        public BoxPlotGraph() {
            
        }

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
        
        public string GetTitle() {
            return _titleLabel.text;
        }
        
        public void SetTitle(string title) {
            _titleLabel.text = title;
        }
        
        // New: add data point & recalc stats
        public void AddDataPoint(float value) {
            _dataPoints.Add(value);

            // Optionally limit data size if you want
            if (_dataPoints.Count > 100000) {
                _dataPoints.RemoveAt(0);
            }
            RecalculateStatistics();
            UpdateBoxPlotDisplay();
        }
        
        public void ClearData() {
            _dataPoints.Clear();
            MinValues.Clear();
            Q1Values.Clear();
            MedianValues.Clear();
            Q3Values.Clear();
            MaxValues.Clear();

            _min = _max = _median = _q1 = _q3 = 0f;
            UpdateBoxPlotDisplay();
        }
        
        private void RecalculateStatistics() {
            if (_dataPoints.Count == 0) {
                _min = _max = _median = _q1 = _q3 = 0f;
                return;
            }
            var sorted = _dataPoints.OrderBy(v => v).ToArray();
            _min = sorted[0];
            _max = sorted[^1];
            _median = CalculateMedian(sorted);

            // Q1
            float[] lowerHalf;
            lowerHalf = sorted.Length % 2 == 0 ? sorted.Take(sorted.Length / 2).ToArray() :
                sorted.Take(sorted.Length / 2 + 1).ToArray();
            _q1 = CalculateMedian(lowerHalf);

            // Q3
            float[] upperHalf;
            upperHalf = sorted.Length % 2 == 0 ? sorted.Skip(sorted.Length / 2).ToArray() :
                sorted.Skip(sorted.Length / 2 + 1).ToArray();
            _q3 = CalculateMedian(upperHalf);
            
            MinValues.Add(_min);
            Q1Values.Add(_q1);
            MedianValues.Add(_median);
            Q3Values.Add(_q3);
            MaxValues.Add(_max);
            
            if (MinValues.Count > 100000) {
                MinValues.RemoveAt(0);
                Q1Values.RemoveAt(0);
                MedianValues.RemoveAt(0);
                Q3Values.RemoveAt(0);
                MaxValues.RemoveAt(0);
            }
        }
        
        private float CalculateMedian(float[] sortedData) {
            int count = sortedData.Length;
            if (count == 0) return 0;
            if (count % 2 == 1) {
                return sortedData[count / 2];
            }
            var middle1 = sortedData[(count / 2) - 1];
            var middle2 = sortedData[count / 2];
            return (middle1 + middle2) / 2f;
        }

        private void UpdateBoxPlotDisplay() {
            if (_dataPoints.Count == 0) return;
            if (_boxplotContainer == null || _minLine == null || _maxLine == null || _box == null ||
                _medianLine == null) {
                Debug.LogError("BoxPlotGraph visual elements are not properly initialized.");
                return;
            }
            
            _maxLabel.text = $"Max: {_max:F2}";
            _q3Label.text = $"Q3: {_q3:F2}";
            _medianLabel.text = $"Med: {_median:F2}";
            _q1Label.text = $"Q1: {_q1:F2}";
            _minLabel.text = $"Min: {_min:F2}";
            
            _boxplotContainer.MarkDirtyRepaint();
            
            schedule.Execute(() => {
                var containerHeight = _boxplotContainer.layout.height > 0 ? _boxplotContainer.layout.height : 200f;
                var range = _max - _min;

                if (Mathf.Approximately(range, 0f)) {
                    // In case all values are the same
                    range = 1f;
                }

                var minPosition = containerHeight * (_min - _min) / range; // always 0
                var maxPosition = containerHeight * (_max - _min) / range;
                var q1Position = containerHeight * (_q1 - _min) / range;
                var medianPosition = containerHeight * (_median - _min) / range;
                var q3Position = containerHeight * (_q3 - _min) / range;
                
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
    }
}