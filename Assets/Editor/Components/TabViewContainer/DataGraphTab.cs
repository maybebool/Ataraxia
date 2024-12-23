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
            BoxPlot1 = new BoxPlotGraph("Beine");
            BoxPlot2 = new BoxPlotGraph("Tremorbewegung");
            BoxPlot3 = new BoxPlotGraph("Head Tremor");
            BoxPlot4 = new BoxPlotGraph("Muskelhypertonie");
            
            lineChartContainer.Add(LineChart);
            boxPlotsContainer.Add(BoxPlot1);
            boxPlotsContainer.Add(BoxPlot2);
            boxPlotsContainer.Add(BoxPlot3);
            boxPlotsContainer.Add(BoxPlot4);
        }
    }
}
