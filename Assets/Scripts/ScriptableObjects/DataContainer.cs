using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ScriptableObjects {
    [CreateAssetMenu(fileName = "DataContainer", menuName = "Scriptable Objects/DataContainers")]
    public class DataContainer : ScriptableObject {
        
        public List<float> minValues = new();
        public List<float> q1Values = new();
        public List<float> medianValues = new();
        public List<float> q3Values = new();
        public List<float> maxValues = new();
        
        public float tremorIntensity;
        public Vector3 currentPos;
        public float degree;
        public List<float> tremorValues = new();
        public float min;
        public float max;
        public float median;
        public float q1;
        public float q3;
        

        public void AddTremorValue(float value) {
            tremorValues.Add(value);

            // Optionally limit the size of tremorValues to a maximum number
            int maxValues = 100000; // Adjust as needed
            if (tremorValues.Count > maxValues) {
                tremorValues.RemoveAt(0);
            }

            // Recalculate statistics whenever a new value is added
            RecalculateStatistics();
        }

        public void RecalculateStatistics() {
            if (tremorValues == null || tremorValues.Count == 0) {
                min = max = median = q1 = q3 = 0f;
                return;
            }

            var sortedValues = tremorValues.OrderBy(v => v).ToArray();
            min = sortedValues[0];
            max = sortedValues[sortedValues.Length - 1];

            var count = sortedValues.Length;
            median = CalculateMedian(sortedValues, count);
            q1 = CalculateMedian(sortedValues.Take(count / 2).ToArray(), count / 2);
            q3 = CalculateMedian(sortedValues.Skip((count + 1) / 2).ToArray(), count / 2);

            // Add the calculated values to the historical lists
            minValues.Add(min);
            q1Values.Add(q1);
            medianValues.Add(median);
            q3Values.Add(q3);
            maxValues.Add(max);

            // Optionally limit the size of the lists
            var maxValuesCount = 100000; // Adjust as needed
            if (minValues.Count > maxValuesCount)
            {
                minValues.RemoveAt(0);
                q1Values.RemoveAt(0);
                medianValues.RemoveAt(0);
                q3Values.RemoveAt(0);
                maxValues.RemoveAt(0);
            }
        }

        private float CalculateMedian(float[] sortedData, int count) {
            if (count == 0) return 0;

            if (count % 2 == 1) {
                return sortedData[count / 2];
            }

            // If even, the median is the average of the two middle elements
            var middle1 = sortedData[(count / 2) - 1];
            var middle2 = sortedData[count / 2];
            return (middle1 + middle2) / 2;
        }
        
        public void ClearData() {
            tremorValues.Clear();
            minValues.Clear();
            q1Values.Clear();
            medianValues.Clear();
            q3Values.Clear();
            maxValues.Clear();
            min = max = median = q1 = q3 = 0f;
        }
    }
}