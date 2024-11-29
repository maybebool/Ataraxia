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
        [SerializeField] private DataContainer scObData;
        private VisualElement _tabContainer;
        private Dictionary<Button, VisualElement> _buttonToUIElementMap = new();
        private List<BoxPlotGraph> _boxPlotGraphs = new();
        private List<DataContainer> _boxPlotDatas = new();
        private double _nextUpdateTime = 0f;
        private double _nextUpdateTimeLineGraph = 0f;
        private float _updateIntervalInSeconds = 0.2f; 
        private float _updateIntervalInSecondsLineGraph = 1f; 
        private float _currentValue = 6f; 

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
                var boxPlotData = CreateInstance<DataContainer>();
            
                boxPlotGraph.SetTitle(t);
            
                _boxPlotGraphs.Add(boxPlotGraph);
                _boxPlotDatas.Add(boxPlotData);
                boxPlotsRow.Add(boxPlotGraph);
            }
            
            graphsContainer.Add(boxPlotsRow);
            _lineChart = new LineGraph("Nervositätslevel");
            graphsContainer.Add(_lineChart);
            rootVisualElement.Add(graphsContainer);
            _buttonToUIElementMap.Add(sceneManagerBtn, tabView);
            _buttonToUIElementMap.Add(dataViewBnt, graphsContainer);

            foreach (var kvp in _buttonToUIElementMap) {
                var button = kvp.Key;
                var uiElement = kvp.Value;
                button.clicked += () => ShowOnlyUIElement(uiElement);
            }
            
            foreach (var element in _buttonToUIElementMap.Values) {
                element.style.display = DisplayStyle.None;
            }
            
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

            _shouldUpdateBoxPlot = (true); 
            if (_shouldUpdateBoxPlot) {
                GenerateRandomDataForBoxPlot(); 
                UpdateBoxPlot();
            }
        }

        private void Update() {
            if (_shouldUpdateBoxPlot) {
                if (EditorApplication.timeSinceStartup >= _nextUpdateTime) {
                    _nextUpdateTime = EditorApplication.timeSinceStartup + _updateIntervalInSeconds;
                    
                    GenerateRandomDataForBoxPlot();
                    UpdateBoxPlot();
                }
            }
            if (EditorApplication.timeSinceStartup >= _nextUpdateTimeLineGraph) {
                _nextUpdateTimeLineGraph = EditorApplication.timeSinceStartup + _updateIntervalInSecondsLineGraph;
                
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
                    Debug.Log(randomValues);
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
            _lineChart.AddDataPoint(scObData.tremorIntensity);
            _lineChart.UpdateChartDisplay();

            // Update current value
            // _currentValue = RandomizeValue(_currentValue);
        }

        private void TestBoxPlotValues() {
            if (_boxPlotDatas.Count > 0) {
                float[] testValues = { 5, 6, 7, 10, 10, 15, 20, 20, 25, 25, 30, 30 };
                _boxPlotDatas[0].values = testValues;
                _boxPlotDatas[0].RecalculateStatistics();
            }
        }
        
        private float RandomizeValue(float value) {
            float randomChange = Random.Range(0.1f, 1f);
            bool increase = Random.value > 0.5f; 
            float newValue = increase ? value + randomChange : value - randomChange; 
            return Mathf.Clamp(newValue, 0.1f, 10f); 
        }
    }
}

