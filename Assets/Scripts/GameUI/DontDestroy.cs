using UnityEngine;

namespace GameUI {
    public class DontDestroy : MonoBehaviour {
    
        [HideInInspector]
        public string objectID;
        private void Awake() {
            objectID = name + "_" + transform.position;
        }

        private void Start() {
            var allDontDestroyObjects = FindObjectsByType<DontDestroy>(FindObjectsSortMode.InstanceID);

            for (var i = 0; i < allDontDestroyObjects.Length; i++) {
                var t = allDontDestroyObjects[i];
                if (t.GetInstanceID() != GetInstanceID()) {
                    if (allDontDestroyObjects[i].objectID == objectID) {
                        var duplicateObject = gameObject;
                        Destroy(duplicateObject);
                        return;
                    }
                }
            }

            DontDestroyOnLoad(gameObject);
        }
    }
}