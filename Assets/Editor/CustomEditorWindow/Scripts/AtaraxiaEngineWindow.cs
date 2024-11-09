using System;
using System.Collections.Generic;
using Editor.CustomEditorWindow.Components.Background;
using Editor.CustomEditorWindow.Components.PlayButton;
using ScriptableObjects;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace Editor.CustomEditorWindow.Scripts {
    public class AtaraxiaEngineWindow : EditorWindow {
        [SerializeField] private VisualTreeAsset visualTreeAsset;
        
        [SerializeField] private VisualTreeAsset backgroundAsset;
        [SerializeField] private VisualTreeAsset upperMainButtonAsset;

        private List<VisualElement> _uiElements;
        private VisualElement _backgroundAssetElement;
        private VisualElement _managerTapViewElement;
        private VisualElement _dataTestElement;


        [SerializeField] private DataContainer obj;
        [SerializeField] private Texture2D backgroundImage;

        private string _scenePath = "Assets/Scenes/TestArea.unity";
        
        private Button _button;

        [MenuItem("Window/Data Testing Editor")]
        public static void ShowWindow() {
            var wnd = GetWindow<AtaraxiaEngineWindow>();
            wnd.titleContent = new GUIContent("Data Testing Window");
        }

        private void OnEnable() {
            
            CreateGUI();
            
            
            

            // var backGround = new BackgroundElements(backgroundImage);
            // backGround.RegisterCallback<MouseDownEvent>(_ => {
            //     Debug.Log("BackGroundClick!");
            // });
            //
            
            
        }

        public void CreateGUI() {
            
            _managerTapViewElement = visualTreeAsset.Instantiate();
            
            // if added via UI Builder (Custom (Project) Competitions)
            var muElem = _managerTapViewElement.Q<BackgroundElements>();
            muElem.SetBackGroundIMage(backgroundImage);
            muElem.RegisterCallback<MouseDownEvent>(_ => {
                Debug.Log("Hello Click!");
            });
            
            this.rootVisualElement.Add(_managerTapViewElement);

            // Load From Code
            // var myElnew = new BackgroundElements();
            // rootVisualElement.Add(myElnew);


        }

        

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