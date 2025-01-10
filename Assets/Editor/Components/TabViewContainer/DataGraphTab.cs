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
        
        public CircleGraph CircleGraph { get; private set; }
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
            BoxPlot1 = new BoxPlotGraph("Rechte Hand");
            BoxPlot2 = new BoxPlotGraph("Linke Hand");
            BoxPlot3 = new BoxPlotGraph("Kopf");
            BoxPlot4 = new BoxPlotGraph("Rechte Hand");
            CircleGraph = new CircleGraph {
                name = "CircleGraph",
                style =
                {
                    width = 100,
                    height = 100,
                    marginLeft = 10,
                    marginTop = 10
                },
                CircleDegree = 270f
            };
            
            lineChartContainer.Add(LineChart);
            boxPlotsContainer.Add(BoxPlot1);
            boxPlotsContainer.Add(BoxPlot2);
            boxPlotsContainer.Add(BoxPlot3);
            boxPlotsContainer.Add(BoxPlot4);
            boxPlotsContainer.Add(CircleGraph);
        }
    }
}
