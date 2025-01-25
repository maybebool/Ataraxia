using System;
using System.IO;
using System.Linq;
using Editor.Components.Buttons;
using Editor.Components.Graphs;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Components.TabViewContainer {
    public class DataGraphTab : VisualElement {
        public LineGraph LineChart { get; private set; }
        public BoxPlotGraph BoxPlot1 { get; private set; }
        public BoxPlotGraph BoxPlot2 { get; private set; }
        public BoxPlotGraph BoxPlot3 { get; private set; }
        public BoxPlotGraph BoxPlot4 { get; private set; }
        public BoxPlotGraph BoxPlot5 { get; private set; }
        public CircleGraph CircleGraphRightHand { get; private set; }
        public CircleGraph CircleGraphLeftHand { get; private set; }

        public DefaultButton SaveButton;

        public DataGraphTab() {
            var dataStyle = Resources.Load<StyleSheet>("Styles/DataGraphsStyle");
            var dataUxml = Resources.Load<VisualTreeAsset>("DataGraphsContainer");
            if (dataStyle != null) {
                styleSheets.Add(dataStyle);
                AddToClassList("custom-data-style");
            }

            if (dataUxml != null) {
                dataUxml.CloneTree(this);
            }
            else {
                Debug.LogError(
                    "Failed to load StyleSheet: BoxPlotStyle.uss.");
            }

            var boxPlotsContainer = this.Q<VisualElement>("BoxPlotsContainer");
            var lineChartContainer = this.Q<VisualElement>("LineGraphContainer");

            LineChart = new LineGraph("Overall Tremor Frequency");
            BoxPlot1 = new BoxPlotGraph("Right Hand", "Tremor Detection");
            BoxPlot2 = new BoxPlotGraph("Left Hand", "Tremor Detection");
            BoxPlot3 = new BoxPlotGraph("Head", "Tremor Detection");
            BoxPlot4 = new BoxPlotGraph("Right Leg", "Tremor Detection");
            BoxPlot5 = new BoxPlotGraph("Left Leg", "Tremor Detection");
            CircleGraphRightHand = new CircleGraph("Right Hand Fingers", "Muscle Tone Detection");
            CircleGraphLeftHand = new CircleGraph("Left Hand Fingers", "Muscle Tone Detection");
            SaveButton = new DefaultButton("Save Results");
            SaveButton.AddToClassList("save-button");
            SaveButton.name = "SaveButton";
            SaveButton.clicked += OnSaveButtonClicked;

            lineChartContainer.Add(LineChart);
            boxPlotsContainer.Add(BoxPlot1);
            boxPlotsContainer.Add(BoxPlot2);
            boxPlotsContainer.Add(BoxPlot3);
            boxPlotsContainer.Add(BoxPlot4);
            boxPlotsContainer.Add(BoxPlot5);
            boxPlotsContainer.Add(CircleGraphRightHand);
            boxPlotsContainer.Add(CircleGraphLeftHand);
            Add(SaveButton);
        }

        private void OnSaveButtonClicked() {
            SaveResults();
        }

        private void SaveResults() {
            var lineChartAverage = CalculateLineChartAverage();
            var csvContent = "Metric,Value\n";
            csvContent += $"Line Chart Average,{lineChartAverage:F2}\n";

            // For each BoxPlot
            var plots = new[] {
                BoxPlot1,
                BoxPlot2,
                BoxPlot3,
                BoxPlot4,
                BoxPlot5
            };

            foreach (var plot in plots) {
                if (plot == null) continue;

                var minValue = plot.GetMinValues().Any() ? plot.GetMinValues().Min() : 0f;
                var q1Average = plot.GetQ1Values().Any() ? plot.GetQ1Values().Average() : 0f;
                var medianAverage = plot.GetMedianValues().Any() ? plot.GetMedianValues().Average() : 0f;
                var q3Average = plot.GetQ3Values().Any() ? plot.GetQ3Values().Average() : 0f;
                var maxValue = plot.GetMaxValues().Any() ? plot.GetMaxValues().Max() : 0f;

                csvContent += $"\n{plot.GetTitle()} Box Plot Data Averages\n";
                csvContent += $"Max,{maxValue:F2}\n";
                csvContent += $"Q3 Average,{q3Average:F2}\n";
                csvContent += $"Median Average,{medianAverage:F2}\n";
                csvContent += $"Q1 Average,{q1Average:F2}\n";
                csvContent += $"Min,{minValue:F2}\n";
            }
            
            csvContent += $"\nCircle Graph Right Hand Percentage,{CircleGraphRightHand.GetCirclePercentage()}%\n";
            csvContent += $"Circle Graph Left Hand Percentage,{CircleGraphLeftHand.GetCirclePercentage()}%\n";

            var folderPath = "Assets/Exports";
            var fileName = $"Results_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
            var fullPath = Path.Combine(folderPath, fileName);

            if (!Directory.Exists(folderPath)) {
                Directory.CreateDirectory(folderPath);
            }

            File.WriteAllText(fullPath, csvContent);
            AssetDatabase.Refresh();
            Debug.Log($"Results saved to {fullPath}");
        }

        private float CalculateLineChartAverage() {
            if (LineChart?.dataPoints == null || LineChart.dataPoints.Count == 0)
                return 0f;

            var sum = LineChart.dataPoints.Sum();
            return sum / LineChart.dataPoints.Count;
        }
    }
}