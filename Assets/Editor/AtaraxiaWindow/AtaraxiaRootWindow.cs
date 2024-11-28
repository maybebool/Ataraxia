using System;
using System.Collections.Generic;
using Editor.Components.Buttons;
using Editor.Components.CenterRowContainer;
using Editor.Components.Graphs;
using Editor.Components.TabViewContainer;
using ScriptableObjects;
using Editor.Helpers;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

namespace Editor.AtaraxiaWindow {
    public class AtaraxiaRootWindow : EditorWindow {
        [SerializeField] private Texture2D backgroundImage;
        [SerializeField] private StyleSheet styleSheet;
        private VisualElement _tabContainer;
        private Dictionary<Button, VisualElement> _buttonToUIElementMap = new();
        private List<BoxPlotGraph> _boxPlotGraphs = new();
        private List<BoxPlotData> _boxPlotDatas = new();
        private double _nextUpdateTime = 0f;
        private double _nextUpdateTimeLineGraph = 0f;
        private float _updateIntervalInSeconds = 0.2f; 
        private float _updateIntervalInSecondsLineGraph = 1f; 
        private float _currentValue = 6f; // Starting value

        private bool _shouldUpdateBoxPlot;
        private LineGraph _lineChart;

        [MenuItem("Window/Ataraxia")]
        public static void ShowWindow() {
            var wnd = GetWindow<AtaraxiaRootWindow>();
            wnd.titleContent = new GUIContent("Ataraxia Manager");
        }

        public void CreateGUI() {
            rootVisualElement.style.backgroundImage = backgroundImage;
            rootVisualElement.styleSheets.Add(styleSheet);
            var sceneManagerBtn = new UpperMainButton("Scene Manager");
            var dataViewBnt = new UpperMainButton("Data View");
            var settingsBnt = new UpperMainButton("Settings");
            var container = new CenterRowContainer(sceneManagerBtn, dataViewBnt, settingsBnt);
            var tabView = new TabViewContainer();

            rootVisualElement.Add(container);
            rootVisualElement.Add(tabView);
            
            var graphsContainer = new VisualElement().AddClass("graphsContainer");
            string[] titles = { "Beine", "Tremorbewegung", "Head Tremor", "Muskelhypertonie" };
            var boxPlotsRow = new VisualElement().AddClass("boxPlotsRow");
            
            foreach (var t in titles)
            {
                var boxPlotGraph = new BoxPlotGraph();
                var boxPlotData = CreateInstance<BoxPlotData>();
            
                boxPlotGraph.SetTitle(t);
                boxPlotGraph.style.display = DisplayStyle.Flex; 
            
                _boxPlotGraphs.Add(boxPlotGraph);
                _boxPlotDatas.Add(boxPlotData);
                boxPlotsRow.Add(boxPlotGraph);
            }
            
            // Add the boxPlotsRow to the boxPlotContainer
            graphsContainer.Add(boxPlotsRow);

            // Create LineChart instance and add it under the boxPlotContainer
            _lineChart = new LineGraph("Nervositätslevel");
            // _lineChart.style.height = 200;
            // _lineChart.style.width = new Length(75, LengthUnit.Percent); 
            // _lineChart.style.marginLeft = 50;// Set desired height
            // _lineChart.style.marginTop = 50;// Set desired height
            // _lineChart.style.alignSelf = Align.FlexStart; // Ensure it's left-aligned
            
            // Add LineChart to boxPlotContainer
            graphsContainer.Add(_lineChart);

            // Add the boxPlotContainer to the root
            rootVisualElement.Add(graphsContainer);

            // Button mapping
            _buttonToUIElementMap.Add(sceneManagerBtn, tabView);
            _buttonToUIElementMap.Add(dataViewBnt, graphsContainer);

            foreach (var kvp in _buttonToUIElementMap) {
                var button = kvp.Key;
                var uiElement = kvp.Value;
                button.clicked += () => ShowOnlyUIElement(uiElement);
            }

            // Initially hide all UI elements except the first
            foreach (var element in _buttonToUIElementMap.Values) {
                element.style.display = DisplayStyle.None;
            }

            // Show the first UI element if available
            var enumerator = _buttonToUIElementMap.Values.GetEnumerator();
            if (enumerator.MoveNext()) {
                var visualElement = enumerator.Current;
                if (visualElement != null)
                    visualElement.style.display = DisplayStyle.Flex;
            }
        }

        private void ShowOnlyUIElement(VisualElement uiElement) {
            foreach (var element in _buttonToUIElementMap.Values) {
                element.style.display = DisplayStyle.None;
            }

            uiElement.style.display = DisplayStyle.Flex;

            _shouldUpdateBoxPlot = (true); // Start/Stop updating if BoxPlotGraphs are shown
            if (_shouldUpdateBoxPlot) {
                GenerateRandomDataForBoxPlot(); // Update initial data on show
                UpdateBoxPlot();
            }
        }

        private void Update() {
            if (_shouldUpdateBoxPlot) {
                if (EditorApplication.timeSinceStartup >= _nextUpdateTime) {
                    _nextUpdateTime = EditorApplication.timeSinceStartup + _updateIntervalInSeconds;

                    // Generate random data and update the box plots
                    GenerateRandomDataForBoxPlot();
                    UpdateBoxPlot();
                }
            }
            if (EditorApplication.timeSinceStartup >= _nextUpdateTimeLineGraph) {
                _nextUpdateTimeLineGraph = EditorApplication.timeSinceStartup + _updateIntervalInSecondsLineGraph;

                // Generate random data and update the box plots
                UpdateLineGraph();
            }
        }

        private void GenerateRandomDataForBoxPlot() {
            var rnd = new System.Random();
            foreach (var data in _boxPlotDatas) {
                if (data != null) {
                    float[] randomValues = new float[50]; // e.g., 50 random values
                    for (int i = 0; i < randomValues.Length; i++) {
                        randomValues[i] = (float)rnd.NextDouble() * 100f;
                    }

                    data.values = randomValues;
                    data.RecalculateStatistics(); 
                    Debug.Log(randomValues);// Recalculate after assigning new values
                }
            }
        }

        private void UpdateBoxPlot() {
            for (int i = 0; i < _boxPlotGraphs.Count; i++) {
                var graph = _boxPlotGraphs[i];
                var data = _boxPlotDatas[i];
                if (graph != null && data != null) {
                    graph.SetBoxPlotData(data);
                }
                
            }
        }

        private void UpdateLineGraph() {
            _lineChart.AddDataPoint(_currentValue);
            _lineChart.UpdateChartDisplay();

            // Update current value
            _currentValue = RandomizeValue(_currentValue);
        }

        private void TestBoxPlotValues() {
            if (_boxPlotDatas.Count > 0) {
                // Hard-coded values representing a distribution
                float[] testValues = { 5, 6, 7, 10, 10, 15, 20, 20, 25, 25, 30, 30 };
                _boxPlotDatas[0].values = testValues;
                _boxPlotDatas[0].RecalculateStatistics(); // Recalculate after assigning new values
            }
        }
        
        private float RandomizeValue(float value) {
            float randomChange = Random.Range(0.1f, 1f); // Random change between 0.1 and 1
            bool increase = Random.value > 0.5f; // Randomly decide whether to increase or decrease the value
            float newValue = increase ? value + randomChange : value - randomChange; // Apply change
            return Mathf.Clamp(newValue, 0.1f, 10f); // Clamp value between 0.1 and 12
        }
    }
}

