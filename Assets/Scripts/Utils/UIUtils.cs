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


        public static void BindEventToMultipleActions<T>(UnityEvent<T> unityEvent, params UnityAction<T>[] unityActions) {
            foreach (var action in unityActions) {
                unityEvent.AddListener(action);
            }
        }
    }
}