using UnityEngine;

namespace ScriptableObjects {
    [CreateAssetMenu(fileName = "DataContainer", menuName = "Scriptable Objects/DataContainers")]
    public class DataContainer : ScriptableObject {
        
        [Header("Data Collector States")]
        public bool isRightHandCollectingData;
        public bool isLeftHandCollectingData;
        public bool isHeadCollectingData;
        
        [Header("Right Hand Data")]
        public float tremorIntensityRightHand;
        public Vector3 rightHandCurrentPos;
        public float rightHandDegree;
        [Header("Right Hand adjustable Parameters")]
        public float rightHandIntensityMultiplier = 0.01f;
        public float rightHandOscillationThreshold = 140f;
        
        [Header("Left Hand Data")]
        public float tremorIntensityLeftHand;
        public Vector3 leftHandCurrentPos;
        public float leftHandDegree;
        [Header("Left Hand adjustable Parameters")]
        public float leftHandIntensityMultiplier = 0.01f;
        public float leftHandOscillationThreshold = 140f;
        
        [Header("Head Data")]
        public float tremorIntensityHead;
        public Vector3 headCurrentPos;
        public float headDegree;
        [Header("Head adjustable Parameters")]
        public float headIntensityMultiplier = 0.03f;
        public float headOscillationThreshold = 110f;
        
        
        public void ClearData() {
            tremorIntensityRightHand = 0f;
            isRightHandCollectingData = false;
        }
    }
}