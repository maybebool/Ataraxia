using ScriptableObjects;
using UnityEngine;

namespace Exercises.ExerciseThree {
    public abstract class MTAToneBase : MonoBehaviour {
        
        public DataContainer scO;
        
        [Header("Scaling Settings (Same Logic)")]
        public float minHeight = 0.5f;
        public float maxHeight = 5f;

        [Tooltip("How fast to smoothly interpolate each frame")]
        public float interpolationSpeed = 1.5f;
        
        
        protected abstract float NewYAxisScaleValue { get; set; }
        protected abstract float BtnPressureValue { get; set; }

        protected virtual void OnEnable() { }
        protected virtual void OnDisable() { }

        private void Update() {
            // Same "growing/shrinking" logic, but user-driven targetT
            var currentY = transform.localScale.y;
            var desiredY = Mathf.Lerp(minHeight, maxHeight, BtnPressureValue);

            // Smoothly interpolate
            NewYAxisScaleValue = Mathf.Lerp(currentY, desiredY, Time.deltaTime * interpolationSpeed);
            var delta = NewYAxisScaleValue - currentY;

            // Shift upward half the delta to grow "upwards"
            transform.localPosition += new Vector3(0, delta * 0.5f, 0);
            transform.localScale = new Vector3(transform.localScale.x, NewYAxisScaleValue, transform.localScale.z);
        }
    }
}