using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObjects {
    [CreateAssetMenu(fileName = "DataContainer", menuName = "Scriptable Objects/DataContainers")]
    public class DataContainer : ScriptableObject {
        public bool isRightHandCollectingData;
        public bool isLeftHandCollectingData;
        public bool isHeadCollectingData;
        
        public float tremorIntensityRightHand;
        public float tremorIntensityLeftHand;
        public float tremorIntensityHead;
        
        public Vector3 rightHandCurrentPos;
        public float rightHandDegree;
        public Vector3 leftHandCurrentPos;
        public float leftHandDegree;
        public Vector3 headCurrentPos;
        public float headDegree;
        
        
        public void ClearData() {
            tremorIntensityRightHand = 0f;
            isRightHandCollectingData = false;
        }
    }
}