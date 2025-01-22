using System.Collections;
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

        private Coroutine _toneCollectionCoroutine;

        protected abstract float NewYAxisScaleValue { get; set; }
        protected abstract float BtnPressureValue { get; set; }
        
        protected virtual void OnEnable() {
        }

        protected virtual void OnDisable() {
        }


        /// <summary>
        /// Updates the local scale and position of the game object based on dynamic target values for the Y-axis.
        /// </summary>
        /// <remarks>
        /// This method adjusts the Y-axis scale of the game object by interpolating between a defined minimum and
        /// maximum height, using the button pressure value as an input. The scaling is performed with smooth
        /// interpolation determined by the specified interpolation speed. Additionally, the method recalculates the
        /// vertical position of the object to maintain alignment with the new scale.
        /// </remarks>
        private void Update()
        {
            var currentY = transform.localScale.y;
            var desiredY = Mathf.Lerp(minHeight, maxHeight, BtnPressureValue);

            NewYAxisScaleValue = Mathf.Lerp(currentY, desiredY, Time.deltaTime * interpolationSpeed);
            var delta = NewYAxisScaleValue - currentY;

            transform.localPosition += new Vector3(0, delta * 0.5f, 0);
            transform.localScale = new Vector3(transform.localScale.x, NewYAxisScaleValue, transform.localScale.z);
        }
    }
}