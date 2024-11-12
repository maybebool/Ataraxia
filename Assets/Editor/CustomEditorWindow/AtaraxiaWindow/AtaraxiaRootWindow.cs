using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.CustomEditorWindow.AtaraxiaWindow {
    public class AtaraxiaRootWindow : EditorWindow
    {
        [SerializeField] private VisualTreeAsset m_VisualTreeAsset = default;
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
            // Each editor window contains a root VisualElement object
            var root = rootVisualElement;
            rootVisualElement.style.backgroundImage = backgroundImage;

            // VisualElements objects can contain other VisualElement following a tree hierarchy.
            VisualElement label = new Label("Hello World! From C#");
            root.Add(label);

            // Instantiate UXML
            VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();
            root.Add(labelFromUXML);
        }
    }
}
