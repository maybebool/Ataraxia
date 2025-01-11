using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Components.SettingsPage {
    public class ParameterSettings : VisualElement {
        
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
            
            // Right Hand Tremor Parameters
            var rHIntensityMultiplier = this.Q<Slider>("MultiplierSlider1");
            var rHIntensityMultiplierValue = this.Q<Label>("MultiplierValue1");
            var rHOscillationThreshold = this.Q<Slider>("OscillationThreshold1");
            var rHOscillationThresholdValue = this.Q<Label>("ThresholdValue1");
            var rHWeighting = this.Q<Slider>("ImortanceWeight1");
            var rHWeightingValue = this.Q<Label>("WeightValue1");
            
            // Left Hand Tremor Parameters
            var lHIntensityMultiplier = this.Q<Slider>("MultiplierSlider2");
            var lHIntensityMultiplierValue = this.Q<Label>("MultiplierValue2");
            var lHOscillationThreshold = this.Q<Slider>("OscillationThreshold2");
            var lHOscillationThresholdValue = this.Q<Label>("ThresholdValue2");
            var lHWeighting = this.Q<Slider>("ImortanceWeight2");
            var lHWeightingValue = this.Q<Label>("WeightValue2");
            
            // Head Tremor Parameters
            var headIntensityMultiplier = this.Q<Slider>("MultiplierSlider3");
            var headIntensityMultiplierValue = this.Q<Label>("MultiplierValue3");
            var headOscillationThreshold = this.Q<Slider>("OscillationThreshold3");
            var headOscillationThresholdValue = this.Q<Label>("ThresholdValue3");
            var headWeighting = this.Q<Slider>("ImortanceWeight3");
            var headWeightingValue = this.Q<Label>("WeightValue3");
            
            // Right Leg Tremor Parameters
            var rLIntensityMultiplier = this.Q<Slider>("MultiplierSlider4");
            var rLIntensityMultiplierValue = this.Q<Label>("MultiplierValue4");
            var rLOscillationThreshold = this.Q<Slider>("OscillationThreshold4");
            var rLOscillationThresholdValue = this.Q<Label>("ThresholdValue4");
            var rLWeighting = this.Q<Slider>("ImortanceWeight4");
            var rLWeightingValue = this.Q<Label>("WeightValue4");
            
            // Left Leg Tremor Parameters
            var lLIntensityMultiplier = this.Q<Slider>("MultiplierSlider5");
            var lLIntensityMultiplierValue = this.Q<Label>("MultiplierValue5");
            var lLOscillationThreshold = this.Q<Slider>("OscillationThreshold5");
            var lLOscillationThresholdValue = this.Q<Label>("ThresholdValue5");
            var lLWeighting = this.Q<Slider>("ImortanceWeight5");
            var lLWeightingValue = this.Q<Label>("WeightValue5");
            
            // Finger Tone Parameters
            var outerOffset = this.Q<Slider>("OuterOffset");
            var innerOffset = this.Q<Slider>("OuterOffset");
            
        }
    }
}