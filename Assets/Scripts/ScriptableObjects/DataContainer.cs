using UnityEngine;

namespace ScriptableObjects {
    [CreateAssetMenu(fileName = "DataContainer", menuName = "Scriptable Objects/DataContainers")]
    public class DataContainer : ScriptableObject {
        public Vector3 CurrentPos;
        public float degree;
        
    }
}

