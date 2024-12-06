using System;
using System.Collections.Generic;
using System.Linq;
using Editor.Components.Buttons;
using Editor.Components.CenterRowContainer;
using Editor.Components.Graphs;
using Editor.Components.TabViewContainer;
using ScriptableObjects;
using Editor.Helpers;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

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
        private float _updateIntervalInSeconds = 1f; 
        private float _updateIntervalInSecondsLineGraph = 0.2f; 
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
            var tabViewExercises = new TabViewContainer();
            // var tabViewSettings = new TabViewContainer();

            rootVisualElement.Add(container);
            rootVisualElement.Add(tabViewExercises);
            // rootVisualElement.Add(tabviewSettings);
            
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
            
            // Create a container for the save button
            var buttonContainer = new VisualElement();
            buttonContainer.style.flexDirection = FlexDirection.Row;
            buttonContainer.style.justifyContent = Justify.Center;
            buttonContainer.style.marginTop = 10;

            // Create the Save button
            var saveButton = new DefaultButton("Save Results");
            saveButton.clicked += OnSaveButtonClicked;
            buttonContainer.Add(saveButton);
            graphsContainer.Add(buttonContainer);

            rootVisualElement.Add(graphsContainer);
            _buttonToUIElementMap.Add(sceneManagerBtn, tabViewExercises);
            _buttonToUIElementMap.Add(dataViewBnt, graphsContainer);
            // _buttonToUIElementMap.Add(settingsBnt, tabviewSettings);

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
        
        private void OnDestroy() {
            scObData.ClearData();
            foreach (var data in _boxPlotDatas) {
                data?.ClearData();
            }
            _lineChart?.ClearData();
        }

        private void ShowOnlyUIElement(VisualElement uiElement) {
            foreach (var element in _buttonToUIElementMap.Values) {
                element.style.display = DisplayStyle.None;
            }

            uiElement.style.display = DisplayStyle.Flex;

            _shouldUpdateBoxPlot = true; 
            if (_shouldUpdateBoxPlot) {
                UpdateBoxPlot();
            }
        }
        
        private void Update() {
            if (scObData != null && scObData.isCollectingData) {
                if (!_shouldUpdateBoxPlot) {
                    _shouldUpdateBoxPlot = true;
                }

                if (EditorApplication.timeSinceStartup >= _nextUpdateTime) {
                    _nextUpdateTime = EditorApplication.timeSinceStartup + _updateIntervalInSeconds;

                    UpdateBoxPlot();
                }

                if (EditorApplication.timeSinceStartup >= _nextUpdateTimeLineGraph) {
                    _nextUpdateTimeLineGraph = EditorApplication.timeSinceStartup + _updateIntervalInSecondsLineGraph;

                    UpdateLineGraph();
                }
            } else {
                if (_shouldUpdateBoxPlot) {
                    _shouldUpdateBoxPlot = false;
                }
            }
        }
        
        private void UpdateBoxPlot() {
            foreach (var data in _boxPlotDatas) {
                if (data != null) {
                    data.AddTremorValue(scObData.tremorIntensity);
                }
            }

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
        }
        
        private void OnSaveButtonClicked() {
            SaveResults();
        }
        
        private void SaveResults() {
            var lineChartAverage = CalculateLineChartAverage();
            var csvContent = "Metric,Value\n";
            csvContent += $"Line Chart Average,{lineChartAverage:F2}\n";

            for (int i = 0; i < _boxPlotDatas.Count; i++) {
                var data = _boxPlotDatas[i];
                var graph = _boxPlotGraphs[i];
                if (data != null && graph != null) {
                    // Calculate averages for each part of the box plot
                    var minValue = data.minValues.Any() ? data.minValues.Min() : 0f;
                    var maxValue = data.maxValues.Any() ? data.maxValues.Max() : 0f;
                    var q1Average = data.q1Values.Any() ? data.q1Values.Average() : 0f;
                    var medianAverage = data.medianValues.Any() ? data.medianValues.Average() : 0f;
                    var q3Average = data.q3Values.Any() ? data.q3Values.Average() : 0f;

                    // Add to CSV content
                    csvContent += $"\n{graph.GetTitle()} Box Plot Data Averages\n";
                    csvContent += $"Max Average,{maxValue:F2}\n";
                    csvContent += $"Q3 Average,{q3Average:F2}\n";
                    csvContent += $"Median Average,{medianAverage:F2}\n";
                    csvContent += $"Q1 Average,{q1Average:F2}\n";
                    csvContent += $"Min Average,{minValue:F2}\n";
                }
            }

            var folderPath = "Assets/Exports";
            var fileName = $"Results_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
            var fullPath = System.IO.Path.Combine(folderPath, fileName);

            if (!System.IO.Directory.Exists(folderPath)) {
                System.IO.Directory.CreateDirectory(folderPath);
            }

            System.IO.File.WriteAllText(fullPath, csvContent);
            AssetDatabase.Refresh();
            Debug.Log($"Results saved to {fullPath}");
        }

        private float CalculateLineChartAverage() {
            if (_lineChart == null || _lineChart.DataPoints == null || _lineChart.DataPoints.Count == 0)
                return 0f;

            var sum = 0f;
            foreach (var value in _lineChart.DataPoints) {
                sum += value;
            }
            return sum / _lineChart.DataPoints.Count;
        }
    }
}

