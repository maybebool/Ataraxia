using Editor.Components.Buttons;
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
        public CircleGraph CircleGraphRightHand { get; private set; }
        public CircleGraph CircleGraphLeftHand { get; private set; }
        
        public DefaultButton SaveButton;
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
    }
}
