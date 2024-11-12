using Editor.Components.CenterRowContainer;
using Editor.Components.TabViewContainer;
using Editor.Components.UpperMainButton;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UpperMainButton = Editor.Components.UpperMainButton;

namespace Editor.AtaraxiaWindow {
    public class AtaraxiaRootWindow : EditorWindow
    {
        [SerializeField] private VisualTreeAsset mVisualTreeAssetCenterRowContainer = default;
        [SerializeField] private VisualTreeAsset mVisualTreeAssetMainButton = default;
        [SerializeField] private StyleSheet styleButton;
        [SerializeField] private Texture2D backgroundImage;

        private VisualElement Container;
        private VisualElement Button;

        [MenuItem("Window/Ataraxia")]
        public static void ShowWindow()
        {
            var wnd = GetWindow<AtaraxiaRootWindow>();
            wnd.titleContent = new GUIContent("Ataraxia Manager");
        }

        public void CreateGUI()
        {
            rootVisualElement.style.backgroundImage = backgroundImage;
            
            var container = new CenterRowContainer();
            var tabView = new TabViewContainer();
            
            rootVisualElement.Add(container);
            rootVisualElement.Add(tabView);
        }
        
//         // if added via UI Builder (Custom (Project) Competitions)
//         var muElem = _managerTapViewElement.Q<BackgroundElements>();
//         muElem.SetBackGroundIMage(backgroundImage);
//         muElem.RegisterCallback<MouseDownEvent>(_ => {
//             Debug.Log("Hello Click!");
//         });
//             
//     this.rootVisualElement.Add(_managerTapViewElement);
//
//     // Load From Code
//     // var myElnew = new BackgroundElements();
//     // rootVisualElement.Add(myElnew);
//
//
// }
//
//         
//
// private void ToggleUIElement(VisualElement uiElement) {
//     foreach (var element in _uiElements) {
//         element.style.display = DisplayStyle.None;
//     }
//
//     uiElement.style.display = DisplayStyle.Flex;
// }
    }
}
