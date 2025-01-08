using UnityEngine;

namespace Exercises.ExerciseThree {
    public class AutoScalingController : MonoBehaviour {
        [Header("Overall Height Bounds")] 
        [SerializeField] private float minHeight = 0.1f;
        [SerializeField] private float maxHeight = 5f;

        [Header("List of Target Heights to Cycle Through")]
        [SerializeField] private float[] targetHeights = { 0.2f, 1f, 1.5f, 0.9f, 2f,1.1f, 4.2f };

        [Tooltip("How fast to smoothly interpolate to each target")]
        [SerializeField] private float interpolationSpeed = 1.5f;

        private int _currentIndex;
        private float _currentTargetY;

        private void Start() {
            if (targetHeights.Length == 0) {
                return;
            }

            // Clamp the first target between minHeight and maxHeight
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

            // Check if we're "close enough" to the target; if so, pick the next one
            if (!(Mathf.Abs(newY - _currentTargetY) < 0.01f)) return;
            _currentIndex++;
            if (_currentIndex >= targetHeights.Length) {
                _currentIndex = 0;
            }
            // Clamp the next target to ensure it’s between minHeight and maxHeight
            _currentTargetY = Mathf.Clamp(targetHeights[_currentIndex], minHeight, maxHeight);
        }
    }
}