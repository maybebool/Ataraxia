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

        [Header("Threshold Offsets")]
        [SerializeField] private float outerThresholdOffset = 1f;
        [SerializeField] private float innerThresholdOffset = 0.6f;

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

            // Smoothly interpolate from current Y scale to our current target
            var oldY = transform.localScale.y;
            var newY = Mathf.Lerp(oldY, _currentTargetY, Time.deltaTime * interpolationSpeed);
            var delta = newY - oldY;

            // Shift upward by half the delta so it appears to grow "up" only
            transform.localPosition += new Vector3(0, delta * 0.5f, 0);

            // Apply that new Y to our localScale
            transform.localScale = new Vector3(transform.localScale.x, newY, transform.localScale.z);

            // Current Height
            heightData.targetObjectCurrentHeight = newY;

            // Outer Thresholds
            heightData.targetObjectOuterHeightThresholdTop = newY + outerThresholdOffset;
            heightData.targetObjectOuterHeightThresholdFloor = newY - outerThresholdOffset;

            // Inner Thresholds
            heightData.targetObjectInnerHeightThresholdTop = newY + innerThresholdOffset;
            heightData.targetObjectInnerHeightThresholdFloor = newY - innerThresholdOffset;

            // Check if we're close enough to the target; if so, pick the next one
            if (Mathf.Abs(newY - _currentTargetY) < 0.01f) {
                _currentIndex++;
                if (_currentIndex >= targetHeights.Length) {
                    _currentIndex = 0;
                }

                _currentTargetY = Mathf.Clamp(targetHeights[_currentIndex], minHeight, maxHeight);
            }
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