using UnityEngine.UIElements;

namespace Editor.Helpers {
    public static class VisualElementExtensions {
        public static VisualElement AddClass(this VisualElement element, string className) {
            element.AddToClassList(className);
            return element;
        }
    }
}