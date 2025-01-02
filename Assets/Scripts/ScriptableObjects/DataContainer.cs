using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObjects {
    [CreateAssetMenu(fileName = "DataContainer", menuName = "Scriptable Objects/DataContainers")]
    public class DataContainer : ScriptableObject {
        public bool isCollectingData;
        
        public float tremorIntensityRightHand;
        public float tremorIntensityLeftHand;
        public float tremorIntensityHead;
        public float tremorIntensityLeftLeg;
        public float tremorIntensityRightLeg;
        
        public Vector3 currentPos;
        public float degree;
        
        
        public void ClearData() {
            tremorIntensityRightHand = 0f;
            isCollectingData = false;
        }
    }
}