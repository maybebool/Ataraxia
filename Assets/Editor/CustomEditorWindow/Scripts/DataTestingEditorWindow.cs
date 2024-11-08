using System;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace Editor.CustomEditorWindow.Scripts {
    public class DataTestingEditorWindow : EditorWindow {
        [SerializeField] private VisualTreeAsset mainButtonsSection;
        [SerializeField] private VisualTreeAsset sceneManagerTabViewSection;
        [SerializeField] private VisualTreeAsset dataTestSection;

        private List<VisualElement> _uiElements;
        private VisualElement _mainButtonsElement;
        private VisualElement _managerTapViewElement;
        private VisualElement _dataTestElement;


        [SerializeField] private DataContainer obj;


        private string _scenePath = "Assets/Scenes/TestArea.unity";

        // private UIDocument _doc;
        private Button _button;

        [MenuItem("Window/Data Testing Editor")]
        public static void ShowWindow() {
            var wnd = GetWindow<DataTestingEditorWindow>();
            wnd.titleContent = new GUIContent("Data Testing Window");
        }

        private void OnEnable() {
            mainButtonsSection =
                AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                    "Assets\\Editor\\CustomEditorWindow\\Sections\\MainButttonSection.uxml");
            sceneManagerTabViewSection =
                AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                    "Assets\\Editor\\CustomEditorWindow\\Sections\\SceneManager\\SceneManagerTapSection.uxml");
            dataTestSection =
                AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                    "Assets\\Editor\\CustomEditorWindow\\Sections\\Data\\TestField.uxml");

            CreateGUI();
        }

        public void CreateGUI() {
            
            rootVisualElement.Clear();
            
            _mainButtonsElement = mainButtonsSection.CloneTree();
            rootVisualElement.Add(_mainButtonsElement);
            
            _managerTapViewElement = sceneManagerTabViewSection.CloneTree();
            _managerTapViewElement.style.display = DisplayStyle.None;
            rootVisualElement.Add(_managerTapViewElement);

            _dataTestElement = dataTestSection.CloneTree();
            _dataTestElement.style.display = DisplayStyle.None;
            rootVisualElement.Add(_dataTestElement);

            _uiElements = new List<VisualElement> {
                _managerTapViewElement,
                _dataTestElement
            };

            var controlPanelButton = _mainButtonsElement.Q<Button>("StartButton");
            var dataPanelButton = _mainButtonsElement.Q<Button>("DataButton");

            if (controlPanelButton != null) {
                controlPanelButton.clicked += () => ToggleUIElement(_managerTapViewElement);
            }
            else {
                Debug.LogError("ToggleButton not found in MainUI.uxml");
            }

            if (dataPanelButton != null) {
                dataPanelButton.clicked += () => ToggleUIElement(_dataTestElement);
            }
            else {
                Debug.LogError("ToggleButton not found in MainUI.uxml");
            }
        }

        // // Each editor window contains a root VisualElement object
        // VisualElement root = rootVisualElement;
        //
        // // VisualElements objects can contain other VisualElement following a tree hierarchy.
        // // VisualElement label = new Label("Hello World! From C#");
        // // root.Add(label);
        //
        // // Instantiate UXML
        // VisualElement labelFromUXML = mainButtonsSection.Instantiate();
        // labelFromUXML.AddToClassList("my-custom-template-container");
        // root.Add(labelFromUXML);
        //
        // // var vectorDataView = m_VisualTreeAsset2.Instantiate();
        // var serObj = new SerializedObject(obj);
        // // var prop = vectorDataView.Q<Vector3Field>("Vector3Field");
        // // prop.BindProperty(serObj.FindProperty(nameof(obj.CurrentPos)));
        //
        // // root.Add(vectorDataView);
        //
        // root.Add(sceneManagerTabViewSection.Instantiate());
        //
        // // root.Add(m_VisualTreeAsset4.Instantiate());
        // //
        // // var btn = root.Q<Button>("startButton");
        // // var btn2 = root.Q<Button>("Exitbutton");
        // //
        // // btn.clicked += StartBtnClickEvent;
        // // btn.clicked += ExitBtnClickEvent;


        private void ToggleUIElement(VisualElement uiElement) {
            foreach (var element in _uiElements) {
                element.style.display = DisplayStyle.None;
            }

            uiElement.style.display = DisplayStyle.Flex;
        }

        private void SetPlayModeStartScene(string scenePath) {
            var myWantedStartScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);
            if (myWantedStartScene != null)
                EditorSceneManager.playModeStartScene = myWantedStartScene;
            else
                Debug.Log("Could not find Scene " + scenePath);
        }

        private void StartBtnClickEvent() {
            SetPlayModeStartScene(_scenePath);
            Debug.Log("Shit clicks");
        }

        private void ExitBtnClickEvent() {
            Application.Quit();
        }
    }
}