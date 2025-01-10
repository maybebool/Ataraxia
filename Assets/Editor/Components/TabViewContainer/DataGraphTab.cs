using Editor.Components.Graphs;
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
        public CircleGraph CircleGraphRechts { get; private set; }
        public CircleGraph CircleGraphLinks { get; private set; }
        public DataGraphTab(){
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
            
            LineChart = new LineGraph("Nervositätslevel");
            BoxPlot1 = new BoxPlotGraph("Right Leg");
            BoxPlot2 = new BoxPlotGraph("Left Leg");
            BoxPlot3 = new BoxPlotGraph("Head");
            BoxPlot4 = new BoxPlotGraph("Right Hand");
            BoxPlot5 = new BoxPlotGraph("Left Hand");
            CircleGraphRechts = new CircleGraph("Tonus Rechts") {
                CircleDegree = 270f,
            };
            CircleGraphLinks = new CircleGraph("Tonus Links") {
                CircleDegree = 310f,
            };
            
            lineChartContainer.Add(LineChart);
            boxPlotsContainer.Add(BoxPlot1);
            boxPlotsContainer.Add(BoxPlot2);
            boxPlotsContainer.Add(BoxPlot3);
            boxPlotsContainer.Add(BoxPlot4);
            boxPlotsContainer.Add(BoxPlot5);
            boxPlotsContainer.Add(CircleGraphRechts);
            boxPlotsContainer.Add(CircleGraphLinks);
        }
    }
}
