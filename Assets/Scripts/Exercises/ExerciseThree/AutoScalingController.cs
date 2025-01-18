using Audio;
using Managers;
using ScriptableObjects;
using UnityEngine;

namespace Exercises.ExerciseThree {
    public class AutoScalingController : MonoBehaviour {
        [Header("Data Container Reference")] 
        [SerializeField] private DataContainer heightData;
        

        [Header("Overall Height Bounds")] 
        [SerializeField] private float minHeight = 0.5f;
        [SerializeField] private float maxHeight = 5f;

        [Header("List of Target Heights to Cycle Through")] 
        [SerializeField] private float[] targetHeights = { 0.7f, 1f, 1.5f, 0.9f, 2f, 1.1f, 4.2f };
        [SerializeField] private float interpolationSpeed = 1.5f;

        private int _reachedHeightsCount = 0;
        private int _currentIndex;
        private float _currentTargetY;

        private void Start() {
            
            if (targetHeights.Length == 0) {
                return;
            }
            _currentTargetY = Mathf.Clamp(targetHeights[0], minHeight, maxHeight);
        }

        private void Update() {
            if (targetHeights.Length == 0) return;
            
            var oldY = transform.localScale.y;
            var newY = Mathf.Lerp(oldY, _currentTargetY, Time.deltaTime * interpolationSpeed);
            var delta = newY - oldY;
            
            transform.localPosition += new Vector3(0, delta * 0.5f, 0);
            transform.localScale = new Vector3(transform.localScale.x, newY, transform.localScale.z);
            
            heightData.targetObjectCurrentHeight = newY;
            heightData.targetObjectOuterHeightThresholdTop = newY + heightData.outerThresholdOffset;
            heightData.targetObjectOuterHeightThresholdFloor = newY - heightData.outerThresholdOffset;
            heightData.targetObjectInnerHeightThresholdTop = newY + heightData.innerThresholdOffset;
            heightData.targetObjectInnerHeightThresholdFloor = newY - heightData.innerThresholdOffset;

            if (!(Mathf.Abs(newY - _currentTargetY) < 0.01f)) return;
            _reachedHeightsCount++;
            MtsEventManager.Instance.onLastIndexReachedUnityEvent?.Invoke();
            if (_reachedHeightsCount >= heightData.amountOfIterationsEx3) {
                AudioController.Instance.PlayAudioClip(3,2);
                _reachedHeightsCount = 0;
                return; 
            }
                
            _currentIndex++;
            if (_currentIndex >= targetHeights.Length) {
                _currentIndex = 0;
            }

            _currentTargetY = Mathf.Clamp(targetHeights[_currentIndex], minHeight, maxHeight);
        }

        private void OnDrawGizmos() {
            if (!heightData) return;
            DrawThresholdLine(heightData.targetObjectOuterHeightThresholdTop, Color.red);
            DrawThresholdLine(heightData.targetObjectOuterHeightThresholdFloor, Color.red);
            DrawThresholdLine(heightData.targetObjectInnerHeightThresholdTop, Color.yellow);
            DrawThresholdLine(heightData.targetObjectInnerHeightThresholdFloor, Color.yellow);
        }

        private void DrawThresholdLine(float scaleValue, Color color) {
            var lineY = transform.position.y + (scaleValue * 0.5f);
            var halfLineLength = 0.5f;

            var leftPoint = new Vector3(transform.position.x - halfLineLength, lineY, transform.position.z);
            var rightPoint = new Vector3(transform.position.x + halfLineLength, lineY, transform.position.z);

            Gizmos.color = color;
            Gizmos.DrawLine(leftPoint, rightPoint);
        }
    }
}