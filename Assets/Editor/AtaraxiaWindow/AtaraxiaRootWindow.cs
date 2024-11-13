using System.Collections.Generic;
using Editor.Components.CenterRowContainer;
using Editor.Components.TabViewContainer;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;


namespace Editor.AtaraxiaWindow {
    public class AtaraxiaRootWindow : EditorWindow {
        
        [SerializeField] private Texture2D backgroundImage;

        private VisualElement tabContainer;
        private VisualElement Button;
        private Dictionary<Button, VisualElement> _buttonToUIElementMap = new();


        [MenuItem("Window/Ataraxia")]
        public static void ShowWindow() {
            var wnd = GetWindow<AtaraxiaRootWindow>();
            wnd.titleContent = new GUIContent("Ataraxia Manager");
        }

        public void CreateGUI() {
            rootVisualElement.style.backgroundImage = backgroundImage;

            var container = new CenterRowContainer();
            var tabView = new TabViewContainer();


            rootVisualElement.Add(container);
            rootVisualElement.Add(tabView);
            _buttonToUIElementMap.Add(container.button1, tabView);

            foreach (var kvp in _buttonToUIElementMap) {
                var button = kvp.Key;
                var uiElement = kvp.Value;
                button.clicked += () => ShowOnlyUIElement(uiElement);
            }

            // Initially hide all UI elements except the first
            foreach (var element in _buttonToUIElementMap.Values) {
                element.style.display = DisplayStyle.None;
            }

            if (!_buttonToUIElementMap.Values.GetEnumerator().MoveNext()) return;
            var visualElement = _buttonToUIElementMap.Values.GetEnumerator().Current;
            if (visualElement != null)
                visualElement.style.display = DisplayStyle.Flex;
        }

        private void ShowOnlyUIElement(VisualElement uiElement) {
            // Hide all UI elements
            foreach (var element in _buttonToUIElementMap.Values) {
                element.style.display = DisplayStyle.None;
            }

            // Show the specified UI element
            uiElement.style.display = DisplayStyle.Flex;
        }
    }
}