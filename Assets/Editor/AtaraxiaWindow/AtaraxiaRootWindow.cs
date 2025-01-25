using System;
using System.Collections.Generic;
using System.Linq;
using Editor.Components.Buttons;
using Editor.Components.CenterRowContainer;
using Editor.Components.TabViewContainer;
using ScriptableObjects;
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
        private double _nextUpdateTime = 0f;
        private double _nextUpdateTimeLineGraph = 0f;
        private float _updateIntervalInSeconds = .3f; 
        private float _updateIntervalInSecondsLineGraph = 0.3f; 
        private float _currentValue = 6f; 

        private bool _shouldUpdateBoxPlot;
        private DataGraphTab _dataGraphTab;

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
            var tabViewSettings = new TabViewSettings();
            var tabDataGraphs = new DataGraphTab();
            _dataGraphTab = tabDataGraphs;

            rootVisualElement.Add(container);
            rootVisualElement.Add(tabViewExercises);
            rootVisualElement.Add(tabViewSettings);
            rootVisualElement.Add(tabDataGraphs);
            
            _buttonToUIElementMap.Add(sceneManagerBtn, tabViewExercises);
            _buttonToUIElementMap.Add(dataViewBnt, tabDataGraphs);
            _buttonToUIElementMap.Add(settingsBnt, tabViewSettings);

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
            _dataGraphTab.BoxPlot1.ClearData();
            _dataGraphTab.BoxPlot2.ClearData();
            _dataGraphTab.BoxPlot3.ClearData();
            _dataGraphTab.BoxPlot4.ClearData();
            _dataGraphTab.BoxPlot5.ClearData();
            _dataGraphTab.LineChart.ClearData();
        }

        private void ShowOnlyUIElement(VisualElement uiElement) {
            foreach (var element in _buttonToUIElementMap.Values) {
                element.style.display = DisplayStyle.None;
            }

            uiElement.style.display = DisplayStyle.Flex;
            _shouldUpdateBoxPlot = true; 
        }
        
        private void Update() {
            if (scObData != null) {
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
            if (scObData.isRightHandCollectingData) {
                _dataGraphTab.BoxPlot1.AddDataPoint(scObData.tremorIntensityRightHand);
            }
            if (scObData.isLeftHandCollectingData) {
                _dataGraphTab.BoxPlot2.AddDataPoint(scObData.tremorIntensityLeftHand);
            }
            if (scObData.isHeadCollectingData) {
                _dataGraphTab.BoxPlot3.AddDataPoint(scObData.tremorIntensityHead);
            }
            if (scObData.isRightLegCollectingData) {
                _dataGraphTab.BoxPlot4.AddDataPoint(scObData.tremorIntensityRightLeg);
            }
            if (scObData.isLeftLegCollectingData) {
                _dataGraphTab.BoxPlot5.AddDataPoint(scObData.tremorIntensityLeftLeg);
            }
            if (scObData.isRightFingerToneCollectingData) {
                _dataGraphTab.CircleGraphRightHand.UpdateCircleThresholds(
                    scObData.targetObjectOuterHeightThresholdTop,
                    scObData.targetObjectOuterHeightThresholdFloor,
                    scObData.targetObjectInnerHeightThresholdTop,
                    scObData.targetObjectInnerHeightThresholdFloor,
                    scObData.rightPlayerObjectHeight);
            }
            if (scObData.isLeftFingerToneCollectingData) {
                _dataGraphTab.CircleGraphLeftHand.UpdateCircleThresholds(
                    scObData.targetObjectOuterHeightThresholdTop,
                    scObData.targetObjectOuterHeightThresholdFloor,
                    scObData.targetObjectInnerHeightThresholdTop,
                    scObData.targetObjectInnerHeightThresholdFloor,
                    scObData.leftPlayerObjectHeight);
            }
        }

        private void UpdateLineGraph() {
            _dataGraphTab.LineChart.AddDataPoint(
                    scObData.tremorIntensityRightHand,
                    scObData.tremorIntensityLeftHand,
                    scObData.tremorIntensityHead,
                    scObData.tremorIntensityRightLeg,
                    scObData.tremorIntensityLeftLeg,
                    
                scObData.isRightHandCollectingData,
                scObData.isLeftHandCollectingData,
                scObData.isHeadCollectingData,
                    scObData.isRightLegCollectingData,
                    scObData.isLeftLegCollectingData,
                    
                    scObData.rightHandTremorImportanceWeight,
                    scObData.leftHandTremorImportanceWeight,
                    scObData.headTremorImportanceWeight,
                    scObData.rightLegTremorImportanceWeight,
                    scObData.leftLegTremorImportanceWeight);
            
            _dataGraphTab.LineChart.UpdateChartDisplay();
        }
    }
}

