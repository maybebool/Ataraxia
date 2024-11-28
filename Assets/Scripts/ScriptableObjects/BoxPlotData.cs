using System.Linq;
using UnityEngine;

namespace ScriptableObjects {
    [CreateAssetMenu(fileName = "BoxPlotData", menuName = "Ataraxia/BoxPlotData", order = 1)]
    public class BoxPlotData : ScriptableObject {
        public float[] values;
        public float min;
        public float max;
        public float median;
        public float q1;
        public float q3;

        // Remove OnValidate. We'll handle computations manually
        public void RecalculateStatistics() {
            if (values == null || values.Length == 0) {
                min = max = median = q1 = q3 = 0f;
                return;
            }

            var sortedValues = values.OrderBy(v => v).ToArray();
            min = sortedValues[0];
            max = sortedValues[sortedValues.Length - 1];
            
            var count = sortedValues.Length;
            median = CalculateMedian(sortedValues, count);
            q1 = CalculateMedian(sortedValues.Take(count / 2).ToArray(), count / 2);
            q3 = CalculateMedian(sortedValues.Skip((count + 1) / 2).ToArray(), count / 2);
        }

        private float CalculateMedian(float[] sortedData, int count) {
            if (count == 0) return 0;

            if (count % 2 == 1) {
                return sortedData[count / 2];
            }
            else {
                // If even, the median is the average of the two middle elements
                var middle1 = sortedData[(count / 2) - 1];
                var middle2 = sortedData[count / 2];
                return (middle1 + middle2) / 2;
            }
        }
    }
}