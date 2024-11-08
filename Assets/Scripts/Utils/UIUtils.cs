using UnityEngine.Events;
using UnityEngine.UI;

namespace Utils {
    public static class UIUtil {
        
        
        public static Button.ButtonClickedEvent BindButtonToSingleAction(Button button, UnityAction unityAction) {
            var evt = new Button.ButtonClickedEvent();
            evt.AddListener(unityAction);
            button.onClick = evt;
            return evt;
        }


        public static void CallMultipleActions(Button button, params UnityAction[] unityAction) {
            foreach (var action in unityAction) {
                button.onClick.AddListener(action);
            }
        }
    }
}