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
        public bool isRightFingerToneCollectingData;
        public bool isLeftFingerToneCollectingData;
        
        [Header("Right Hand Data")]
        public float tremorIntensityRightHand;
        public Vector3 rightHandCurrentPos;
        public float rightHandDegree;
        
        [Header("Right Hand adjustable Parameters")]
        public float rightHandIntensityMultiplier = 0.01f;
        public int rightHandOscillationThreshold = 140;
        public float rightHandTremorImportanceWeight = 1f;
        
        [Header("Left Hand Data")]
        public float tremorIntensityLeftHand;
        public Vector3 leftHandCurrentPos;
        public float leftHandDegree;
        
        [Header("Left Hand adjustable Parameters")]
        public float leftHandIntensityMultiplier = 0.01f;
        public int leftHandOscillationThreshold = 140;
        public float leftHandTremorImportanceWeight = 1f;
        
        [Header("Head Data")]
        public float tremorIntensityHead;
        public Vector3 headCurrentPos;
        public float headDegree;
        
        [Header("Head adjustable Parameters")]
        public float headIntensityMultiplier = 0.03f;
        public int headOscillationThreshold = 110;
        public float headTremorImportanceWeight = 1f;
        
        
        [Header("Right Leg Data")]
        public float tremorIntensityRightLeg;
        public Vector3 rightLegCurrentPos;
        public float rightLegDegree;
        
        [Header("Right Leg adjustable Parameters")]
        public float rightLegIntensityMultiplier = 0.03f;
        public int rightLegOscillationThreshold = 110;
        public float rightLegTremorImportanceWeight = 1f;
        
        [Header("Left Leg Data")]
        public float tremorIntensityLeftLeg;
        public Vector3 leftLegCurrentPos;
        public float leftLegDegree;
        
        [Header("Left Leg adjustable Parameters")]
        public float leftLegIntensityMultiplier = 0.03f;
        public int leftLegOscillationThreshold = 110;
        public float leftLegTremorImportanceWeight = 1f;

        [Header("Finger Tone Data")] 
        public float rightPlayerObjectHeight;
        public float leftPlayerObjectHeight;
        public float targetObjectCurrentHeight;
        public float outerThresholdOffset = 1.0f;
        public float innerThresholdOffset = 0.6f;
        public float targetObjectOuterHeightThresholdTop;
        public float targetObjectOuterHeightThresholdFloor;
        public float targetObjectInnerHeightThresholdTop;
        public float targetObjectInnerHeightThresholdFloor;


        [Header("Exercise Variables")] 
        public int amountOfTargetsToCollectEx1;
        public int amountOfTargetsToCollectEx2;
        public int amountOfIterationsEx3;
        
        public void ClearData() {
            tremorIntensityRightHand = 0f;
            isRightHandCollectingData = false;
        }
    }
}