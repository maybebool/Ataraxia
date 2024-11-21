using System;
using System.Collections.Generic;
using Editor.Components.Buttons;
using Editor.Components.CenterRowContainer;
using Editor.Components.Graphs;
using Editor.Components.TabViewContainer;
using ScriptableObjects;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.AtaraxiaWindow {
    public class AtaraxiaRootWindow : EditorWindow {
        [SerializeField] private Texture2D backgroundImage;

        private VisualElement tabContainer;
        private Dictionary<Button, VisualElement> _buttonToUIElementMap = new();
        private double _nextUpdateTime = 0f;
        private float _updateTimer = 0f; // Timer to track elapsed time
        private float _updateIntervalInSeconds = 0.2f; // Set desired update interval here

        private BoxPlotGraph _boxPlotGraph;
        private BoxPlotData _boxPlotData;

        private bool _shouldUpdateBoxPlot;

        [MenuItem("Window/Ataraxia")]
        public static void ShowWindow() {
            var wnd = GetWindow<AtaraxiaRootWindow>();
            wnd.titleContent = new GUIContent("Ataraxia Manager");
        }

        public void CreateGUI() {
            rootVisualElement.style.backgroundImage = backgroundImage;

            var sceneManagerBtn = new UpperMainButton("Scene Manager");
            var dataViewBnt = new UpperMainButton("Data View");
            var container = new CenterRowContainer(sceneManagerBtn,
                dataViewBnt,
                new UpperMainButton("Settings"));
            var tabView = new TabViewContainer();

            rootVisualElement.Add(container);
            rootVisualElement.Add(tabView);

            // Prepare the BoxPlotGraph visual element
            _boxPlotGraph = new BoxPlotGraph();
            _boxPlotData = CreateInstance<BoxPlotData>();

            // Set initial random data for demonstration
            GenerateRandomDataForBoxPlot();

            // We add the box plot to the root (or another container as needed)
            rootVisualElement.Add(_boxPlotGraph);
            _boxPlotGraph.style.display = DisplayStyle.None; // Initially hidden

            // Button mapping
            _buttonToUIElementMap.Add(sceneManagerBtn, tabView);
            _buttonToUIElementMap.Add(dataViewBnt, _boxPlotGraph);

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

            _shouldUpdateBoxPlot = (uiElement == _boxPlotGraph); // Start/Stop updating if BoxPlotGraph is shown
            if (_shouldUpdateBoxPlot) {
                GenerateRandomDataForBoxPlot(); // Update initial data on show
                UpdateBoxPlot();
            }
        }

        private void Update() {
            if (_shouldUpdateBoxPlot) {
                if (EditorApplication.timeSinceStartup >= _nextUpdateTime) {
                    _nextUpdateTime = EditorApplication.timeSinceStartup + _updateIntervalInSeconds;

                    // Generate random data and update the box plot
                    GenerateRandomDataForBoxPlot();
                    UpdateBoxPlot();
                }
            }
        }

        private void GenerateRandomDataForBoxPlot() {
            if (_boxPlotData != null) {
                // Generate random values between 0 and 100 for demonstration
                float[] randomValues = new float[50]; // e.g., 50 random values
                var rnd = new System.Random();
                for (int i = 0; i < randomValues.Length; i++) {
                    randomValues[i] = (float)rnd.NextDouble() * 100f;
                }

                _boxPlotData.values = randomValues;
                _boxPlotData.RecalculateStatistics(); // Recalculate after assigning new values
                Debug.Log(
                    $"Random data generated - min: {_boxPlotData.min}, max: {_boxPlotData.max}, median: {_boxPlotData.median}, q1: {_boxPlotData.q1}, q3: {_boxPlotData.q3}");
            }
        }

        private void UpdateBoxPlot() {
            if (_boxPlotGraph != null && _boxPlotData != null) {
                _boxPlotGraph.SetBoxPlotData(_boxPlotData);
            }
        }

        private void TestBoxPlotValues() {
            if (_boxPlotData != null) {
                // Hard-coded values representing a distribution
                float[] testValues = new float[] { 5, 6, 7, 10, 10, 15, 20, 20, 25, 25, 30, 30 };
                _boxPlotData.values = testValues;
                _boxPlotData.RecalculateStatistics(); // We'll introduce a method to recalculate
                Debug.Log(
                    $"Test values set - min: {_boxPlotData.min}, max: {_boxPlotData.max}, median: {_boxPlotData.median}, q1: {_boxPlotData.q1}, q3: {_boxPlotData.q3}");
            }
        }
    }
}