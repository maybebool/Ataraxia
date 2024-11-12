using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.AtaraxiaWindow {
    public class AtaraxiaRootWindow : EditorWindow
    {
        [SerializeField] private VisualTreeAsset mVisualTreeAssetCenterRowContainer = default;
        [SerializeField] private VisualTreeAsset mVisualTreeAssetMainButton = default;
        [SerializeField] private Texture2D backgroundImage;

        [MenuItem("Window/Ataraxia")]
        public static void ShowWindow()
        {
            var wnd = GetWindow<AtaraxiaRootWindow>();
            wnd.titleContent = new GUIContent("Ataraxia Manager");
        }

        private void OnEnable() {
            CreateGUI();
        }

        public void CreateGUI()
        {
            var root = rootVisualElement;
            rootVisualElement.style.backgroundImage = backgroundImage;
            
            // var UpperContainer = mVisualTreeAssetCenterRowContainer.Q<>
            // var myElnew = new BackgroundElements();
            // rootVisualElement.Add(myElnew);

            // VisualElements objects can contain other VisualElement following a tree hierarchy.
            // var divUpperButton = new VisualElement();
            // divUpperButton.style.flexDirection = FlexDirection.Row;
            // divUpperButton.Add();
            // root.Add(label);

            // Instantiate UXML
            // VisualElement labelFromUXML = mVisualTreeAsset.Instantiate();
            // root.Add(labelFromUXML);
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
