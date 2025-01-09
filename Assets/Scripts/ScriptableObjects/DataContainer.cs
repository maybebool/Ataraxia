using UnityEngine;

namespace ScriptableObjects {
    [CreateAssetMenu(fileName = "DataContainer", menuName = "Scriptable Objects/DataContainers")]
    public class DataContainer : ScriptableObject {
        
        [Header("Data Collector States")]
        public bool isRightHandCollectingData;
        public bool isLeftHandCollectingData;
        public bool isHeadCollectingData;
        public bool isRightLegCollectingData;
        public bool isLeftLegCollectingData;
        
        [Header("Right Hand Data")]
        public float tremorIntensityRightHand;
        public Vector3 rightHandCurrentPos;
        public float rightHandDegree;
        
        [Header("Right Hand adjustable Parameters")]
        public float rightHandIntensityMultiplier = 0.01f;
        public float rightHandOscillationThreshold = 140f;
        public float rightHandTremorImportanceWeight = 1f;
        
        [Header("Left Hand Data")]
        public float tremorIntensityLeftHand;
        public Vector3 leftHandCurrentPos;
        public float leftHandDegree;
        
        [Header("Left Hand adjustable Parameters")]
        public float leftHandIntensityMultiplier = 0.01f;
        public float leftHandOscillationThreshold = 140f;
        public float leftHandTremorImportanceWeight = 1f;
        
        [Header("Head Data")]
        public float tremorIntensityHead;
        public Vector3 headCurrentPos;
        public float headDegree;
        
        [Header("Head adjustable Parameters")]
        public float headIntensityMultiplier = 0.03f;
        public float headOscillationThreshold = 110f;
        public float headTremorImportanceWeight = 1f;
        
        
        [Header("Right Leg Data")]
        public float tremorIntensityRightLeg;
        public Vector3 rightLegCurrentPos;
        public float rightLegDegree;
        
        [Header("Right Leg adjustable Parameters")]
        public float rightLegIntensityMultiplier = 0.03f;
        public float rightLegOscillationThreshold = 110f;
        public float rightLegTremorImportanceWeight = 1f;
        
        [Header("Left Leg Data")]
        public float tremorIntensityLeftLeg;
        public Vector3 leftLegCurrentPos;
        public float leftLegDegree;
        
        [Header("Left Leg adjustable Parameters")]
        public float leftLegIntensityMultiplier = 0.03f;
        public float leftLegOscillationThreshold = 110f;
        public float leftLegTremorImportanceWeight = 1f;

        [Header("Finger Tone Data")] 
        public float rightPlayerObjectHeight;
        public float leftPlayerObjectHeight;
        public float targetObjectCurrentHeight;
        public float targetObjectOuterHeightThresholdTop;
        public float targetObjectOuterHeightThresholdFloor;
        public float targetObjectInnerHeightThresholdTop;
        public float targetObjectInnerHeightThresholdFloor;
        
        public void ClearData() {
            tremorIntensityRightHand = 0f;
            isRightHandCollectingData = false;
        }
    }
}