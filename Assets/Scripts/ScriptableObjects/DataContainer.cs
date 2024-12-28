
using UnityEngine;

namespace ScriptableObjects {
    [CreateAssetMenu(fileName = "DataContainer", menuName = "Scriptable Objects/DataContainers")]
    public class DataContainer : ScriptableObject {
        public bool isCollectingData;
        public float tremorIntensity;
        public Vector3 currentPos;
        public float degree;
        
        
        public void ClearData() {
            tremorIntensity = 0f;
            isCollectingData = false;
        }
    }
}