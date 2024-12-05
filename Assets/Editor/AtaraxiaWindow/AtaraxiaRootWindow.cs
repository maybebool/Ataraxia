using System;
using System.Collections.Generic;
using System.Linq;
using Editor.Components.Buttons;
using Editor.Components.CenterRowContainer;
using Editor.Components.Graphs;
using Editor.Components.TabViewContainer;
using ScriptableObjects;
using Editor.Helpers;
using Managers;
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
        
        private void OnEnable() {
            // Subscribe to events from EventManager
            if (MtsEventManager.Instance != null) {
                MtsEventManager.Instance.OnButtonPressed += StartDataCollection;
                MtsEventManager.Instance.OnButtonReleased += StopDataCollection;
            }
        }

        private void OnDisable() {
            // Unsubscribe from events
            if (MtsEventManager.Instance != null) {
                MtsEventManager.Instance.OnButtonPressed -= StartDataCollection;
                MtsEventManager.Instance.OnButtonReleased -= StopDataCollection;
            }
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
            // Create a container for the save button
            var buttonContainer = new VisualElement();
            buttonContainer.style.flexDirection = FlexDirection.Row;
            buttonContainer.style.justifyContent = Justify.Center;
            buttonContainer.style.marginTop = 10;

            // Create the Save button
            var saveButton = new DefaultButton("Save Results");
            // saveButton.text = "Save Results";
            saveButton.clicked += OnSaveButtonClicked;

            // Add the button to the container
            buttonContainer.Add(saveButton);

            // Add the button container to the graphsContainer
            graphsContainer.Add(buttonContainer);

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
        
        private void StartDataCollection() {
            _shouldUpdateBoxPlot = true;
            // Optionally reset data if needed
            // ResetData();
        }

        private void StopDataCollection() {
            _shouldUpdateBoxPlot = false;
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

            _shouldUpdateBoxPlot = (true); 
            if (_shouldUpdateBoxPlot) {
                UpdateBoxPlot();
            }
        }

        private void Update() {
            if (_shouldUpdateBoxPlot) {
                if (EditorApplication.timeSinceStartup >= _nextUpdateTime) {
                    _nextUpdateTime = EditorApplication.timeSinceStartup + _updateIntervalInSeconds;

                    UpdateBoxPlot();
                    // Debug logs if needed
                    Debug.Log(scObData.median);
                    Debug.Log(scObData.max);
                    Debug.Log(scObData.q3);
                    Debug.Log(scObData.q1);
                }

                if (EditorApplication.timeSinceStartup >= _nextUpdateTimeLineGraph) {
                    _nextUpdateTimeLineGraph = EditorApplication.timeSinceStartup + _updateIntervalInSecondsLineGraph;

                    UpdateLineGraph();
                }
            }
        }
        // private void Update() {
        //     if (_shouldUpdateBoxPlot) {
        //         if (EditorApplication.timeSinceStartup >= _nextUpdateTime) {
        //             _nextUpdateTime = EditorApplication.timeSinceStartup + _updateIntervalInSeconds;
        //             
        //             UpdateBoxPlot();
        //             Debug.Log(scObData.median);
        //             Debug.Log(scObData.max);
        //             Debug.Log(scObData.q3);
        //             Debug.Log(scObData.q1);
        //         }
        //     }
        //     if (EditorApplication.timeSinceStartup >= _nextUpdateTimeLineGraph) {
        //         _nextUpdateTimeLineGraph = EditorApplication.timeSinceStartup + _updateIntervalInSecondsLineGraph;
        //         
        //         UpdateLineGraph();
        //     }
        //     
        //     Debug.Log(scObData.maxValues.Count);
        // }

        private void UpdateBoxPlot() {
            if (!_shouldUpdateBoxPlot) return;

            // For demonstration, we'll use the same tremorIntensity for all box plots
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
            // // For demonstration, we'll use the same tremorIntensity for all box plots
            // // In the future, these can be replaced with different data sources
            // foreach (var data in _boxPlotDatas) {
            //     if (data != null) {
            //         data.AddTremorValue(scObData.tremorIntensity);
            //     }
            // }
            //
            // for (int i = 0; i < _boxPlotGraphs.Count; i++) {
            //     var graph = _boxPlotGraphs[i];
            //     var data = _boxPlotDatas[i];
            //
            //     if (graph != null && data != null) {
            //         graph.SetBoxPlotData(data);
            //     }
            // }
        }

        private void UpdateLineGraph() {
            if (!_shouldUpdateBoxPlot) return;
            _lineChart.AddDataPoint(scObData.tremorIntensity);
            _lineChart.UpdateChartDisplay();
            // _lineChart.AddDataPoint(scObData.tremorIntensity);
            // _lineChart.UpdateChartDisplay();
        }
        
        private void OnSaveButtonClicked()
        {
            SaveResults();
        }
        
        private void SaveResults()
        {
            // Calculate line chart average
            float lineChartAverage = CalculateLineChartAverage();

            // Prepare CSV data
            string csvContent = "Metric,Value\n";
            csvContent += $"Line Chart Average,{lineChartAverage:F2}\n";

            // Loop over each box plot data
            for (int i = 0; i < _boxPlotDatas.Count; i++)
            {
                var data = _boxPlotDatas[i];
                var graph = _boxPlotGraphs[i];
                if (data != null && graph != null)
                {
                    // Calculate averages for each part of the box plot
                    var minAverage = data.minValues.Average();
                    var q1Average = data.q1Values.Average();
                    var medianAverage = data.medianValues.Average();
                    var q3Average = data.q3Values.Average();
                    var maxAverage = data.maxValues.Average();

                    // Add to CSV content
                    csvContent += $"\n{graph.GetTitle()} Box Plot Data Averages\n";
                    csvContent += $"Min Average,{minAverage:F2}\n";
                    csvContent += $"Q1 Average,{q1Average:F2}\n";
                    csvContent += $"Median Average,{medianAverage:F2}\n";
                    csvContent += $"Q3 Average,{q3Average:F2}\n";
                    csvContent += $"Max Average,{maxAverage:F2}\n";
                }
            }

            // Define the file path
            var folderPath = "Assets/Exports";
            var fileName = $"Results_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
            var fullPath = System.IO.Path.Combine(folderPath, fileName);

            // Ensure the directory exists
            if (!System.IO.Directory.Exists(folderPath)) {
                System.IO.Directory.CreateDirectory(folderPath);
            }

            // Write the CSV file
            System.IO.File.WriteAllText(fullPath, csvContent);
            AssetDatabase.Refresh();
            Debug.Log($"Results saved to {fullPath}");
        }
        
        private float CalculateLineChartAverage()
        {
            if (_lineChart == null || _lineChart.DataPoints == null || _lineChart.DataPoints.Count == 0)
                return 0f;

            float sum = 0f;
            foreach (var value in _lineChart.DataPoints)
            {
                sum += value;
            }

            return sum / _lineChart.DataPoints.Count;
        }
        
        private float CalculateBoxPlotAverage()
        {
            if (scObData == null || scObData.tremorValues == null || scObData.tremorValues.Count == 0)
                return 0f;

            var sortedValues = scObData.tremorValues.OrderBy(v => v).ToList();
            int count = sortedValues.Count;

            if (count % 2 == 1)
            {
                // Return median for odd count
                return sortedValues[count / 2];
            }
            else
            {
                // Return median for even count
                float middle1 = sortedValues[(count / 2) - 1];
                float middle2 = sortedValues[count / 2];
                return (middle1 + middle2) / 2f;
            }
        }
    }
}

