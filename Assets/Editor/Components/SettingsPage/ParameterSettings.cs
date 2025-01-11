using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Components.SettingsPage {
    public class ParameterSettings : VisualElement {
        
        private const string IntFormat = "0";
        private const string FloatFormat = "0.0";
        private const string DoubleFloatFormat = "0.00";
        
        public ParameterSettings() {
            var parameters = Resources.Load<StyleSheet>("Styles/ParametersStyle");
            var parametersUxml = Resources.Load<VisualTreeAsset>("ParametersContainer");
            if (parameters != null) {
                styleSheets.Add(parameters);
                AddToClassList("custom-parameter-settings-container");
            }
            if (parametersUxml != null) {
                parametersUxml.CloneTree(this);
                
            }else {
                Debug.LogError(
                    "Failed to load StyleSheet: BoxPlotStyle.uss.");
            }
            
            #region Slider Events
            
            // Right Hand Tremor Parameters
            var rHIntensityMultiplier = this.Q<Slider>("MultiplierSlider1");
            var rHIntensityMultiplierValue = this.Q<Label>("MultiplierValue1");
            SliderEvent(rHIntensityMultiplier,rHIntensityMultiplierValue, DoubleFloatFormat);
            
            var rHOscillationThreshold = this.Q<Slider>("OscillationThreshold1");
            var rHOscillationThresholdValue = this.Q<Label>("ThresholdValue1");
            SliderEvent(rHOscillationThreshold,rHOscillationThresholdValue, IntFormat);
            
            var rHWeighting = this.Q<Slider>("ImortanceWeight1");
            var rHWeightingValue = this.Q<Label>("WeightValue1");
            SliderEvent(rHWeighting,rHWeightingValue, FloatFormat);
            
            // Left Hand Tremor Parameters
            var lHIntensityMultiplier = this.Q<Slider>("MultiplierSlider2");
            var lHIntensityMultiplierValue = this.Q<Label>("MultiplierValue2");
            SliderEvent(lHIntensityMultiplier,lHIntensityMultiplierValue, DoubleFloatFormat);
            
            var lHOscillationThreshold = this.Q<Slider>("OscillationThreshold2");
            var lHOscillationThresholdValue = this.Q<Label>("ThresholdValue2");
            SliderEvent(lHOscillationThreshold,lHOscillationThresholdValue, IntFormat);
            
            var lHWeighting = this.Q<Slider>("ImortanceWeight2");
            var lHWeightingValue = this.Q<Label>("WeightValue2");
            SliderEvent(lHWeighting,lHWeightingValue, FloatFormat);
            
            // Head Tremor Parameters
            var headIntensityMultiplier = this.Q<Slider>("MultiplierSlider3");
            var headIntensityMultiplierValue = this.Q<Label>("MultiplierValue3");
            SliderEvent(headIntensityMultiplier,headIntensityMultiplierValue, DoubleFloatFormat);
            
            var headOscillationThreshold = this.Q<Slider>("OscillationThreshold3");
            var headOscillationThresholdValue = this.Q<Label>("ThresholdValue3");
            SliderEvent(headOscillationThreshold,headOscillationThresholdValue, IntFormat);
            
            var headWeighting = this.Q<Slider>("ImortanceWeight3");
            var headWeightingValue = this.Q<Label>("WeightValue3");
            SliderEvent(headWeighting,headWeightingValue, FloatFormat);
            
            // Right Leg Tremor Parameters
            var rLIntensityMultiplier = this.Q<Slider>("MultiplierSlider4");
            var rLIntensityMultiplierValue = this.Q<Label>("MultiplierValue4");
            SliderEvent(rLIntensityMultiplier,rLIntensityMultiplierValue, DoubleFloatFormat);
            
            var rLOscillationThreshold = this.Q<Slider>("OscillationThreshold4");
            var rLOscillationThresholdValue = this.Q<Label>("ThresholdValue4");
            SliderEvent(rLOscillationThreshold,rLOscillationThresholdValue, IntFormat);
            
            var rLWeighting = this.Q<Slider>("ImortanceWeight4");
            var rLWeightingValue = this.Q<Label>("WeightValue4");
            SliderEvent(rLWeighting,rLWeightingValue, FloatFormat);
            
            // Left Leg Tremor Parameters
            var lLIntensityMultiplier = this.Q<Slider>("MultiplierSlider5");
            var lLIntensityMultiplierValue = this.Q<Label>("MultiplierValue5");
            SliderEvent(lLIntensityMultiplier,lLIntensityMultiplierValue, DoubleFloatFormat);
            
            var lLOscillationThreshold = this.Q<Slider>("OscillationThreshold5");
            var lLOscillationThresholdValue = this.Q<Label>("ThresholdValue5");
            SliderEvent(lLOscillationThreshold,lLOscillationThresholdValue, IntFormat);
            
            var lLWeighting = this.Q<Slider>("ImortanceWeight5");
            var lLWeightingValue = this.Q<Label>("WeightValue5");
            SliderEvent(lLWeighting,lLWeightingValue, FloatFormat);
            
            // Finger Tone Parameters
            var outerOffset = this.Q<Slider>("OuterOffset");
            var outerOffsetValue = this.Q<Label>("OuterOffsetValue");
            SliderEvent(outerOffset,outerOffsetValue, FloatFormat);
            
            var innerOffset = this.Q<Slider>("OuterOffset");
            var innerOffsetValue = this.Q<Label>("InnerOffsetValue");
            SliderEvent(innerOffset,innerOffsetValue, FloatFormat);
            
            #endregion
        }
        private void SliderEvent(Slider triggeredSlider, Label label, string format) {
            triggeredSlider.RegisterValueChangedCallback(evt => {
                if (label != null) {
                    label.text = evt.newValue.ToString(format);
                }
            });
        }
    }
}