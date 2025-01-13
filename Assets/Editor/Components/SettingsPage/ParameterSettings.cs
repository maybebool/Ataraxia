using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Components.SettingsPage {
    public class ParameterSettings : VisualElement {
        
        private const string IntFormat = "0";
        private const string FloatFormat = "0.0";
        private const string DoubleFloatFormat = "0.00";
        private const float FloatStepSize = 0.1f;
        private const float DoubleFloatStepSize = 0.01f;
        
        
        [System.Serializable]
        private struct SliderParam {
            public string sliderName;   
            public string labelName;    
            public float defaultValue;  
            public string defaultText;  
            public float stepSize;      
            public string format;       
        }

        
        private SliderParam[] sliderParams = {
            // Right Hand Tremor
            new() {
                sliderName   = "MultiplierSlider1",
                labelName    = "MultiplierValue1",
                defaultValue = 0.03f,
                defaultText  = "0.03",
                stepSize     = DoubleFloatStepSize,
                format       = DoubleFloatFormat
            },
            new() {
                sliderName   = "OscillationThreshold1",
                labelName    = "ThresholdValue1",
                defaultValue = 140,
                defaultText  = "140",
                stepSize     = 1,  
                format       = IntFormat
            },
            new() {
                sliderName   = "ImortanceWeight1",
                labelName    = "WeightValue1",
                defaultValue = 1f,
                defaultText  = "1.0",
                stepSize     = FloatStepSize,
                format       = FloatFormat
            },

            // Left Hand Tremor
            new() {
                sliderName   = "MultiplierSlider2",
                labelName    = "MultiplierValue2",
                defaultValue = 0.03f,
                defaultText  = "0.03",
                stepSize     = DoubleFloatStepSize,
                format       = DoubleFloatFormat
            },
            new() {
                sliderName   = "OscillationThreshold2",
                labelName    = "ThresholdValue2",
                defaultValue = 140,
                defaultText  = "140",
                stepSize     = 1,
                format       = IntFormat
            },
            new() {
                sliderName   = "ImortanceWeight2",
                labelName    = "WeightValue2",
                defaultValue = 1f,
                defaultText  = "1.0",
                stepSize     = FloatStepSize,
                format       = FloatFormat
            },

            // Head Tremor
            new() {
                sliderName   = "MultiplierSlider3",
                labelName    = "MultiplierValue3",
                defaultValue = 0.03f,
                defaultText  = "0.03",
                stepSize     = DoubleFloatStepSize,
                format       = DoubleFloatFormat
            },
            new() {
                sliderName   = "OscillationThreshold3",
                labelName    = "ThresholdValue3",
                defaultValue = 110,
                defaultText  = "110",
                stepSize     = 1,
                format       = IntFormat
            },
            new() {
                sliderName   = "ImortanceWeight3",
                labelName    = "WeightValue3",
                defaultValue = 1f,
                defaultText  = "1.0",
                stepSize     = FloatStepSize,
                format       = FloatFormat
            },

            // Right Leg Tremor
            new() {
                sliderName   = "MultiplierSlider4",
                labelName    = "MultiplierValue4",
                defaultValue = 0.03f,
                defaultText  = "0.03",
                stepSize     = DoubleFloatStepSize,
                format       = DoubleFloatFormat
            },
            new() {
                sliderName   = "OscillationThreshold4",
                labelName    = "ThresholdValue4",
                defaultValue = 110,
                defaultText  = "110",
                stepSize     = 1,
                format       = IntFormat
            },
            new() {
                sliderName   = "ImportanceWeight4",
                labelName    = "WeightValue4",
                defaultValue = 1f,
                defaultText  = "1.0",
                stepSize     = FloatStepSize,
                format       = FloatFormat
            },

            // Left Leg Tremor
            new() {
                sliderName   = "MultiplierSlider5",
                labelName    = "MultiplierValue5",
                defaultValue = 0.03f,
                defaultText  = "0.03",
                stepSize     = DoubleFloatStepSize,
                format       = DoubleFloatFormat
            },
            new() {
                sliderName   = "OscillationThreshold5",
                labelName    = "ThresholdValue5",
                defaultValue = 110,
                defaultText  = "110",
                stepSize     = 1,
                format       = IntFormat
            },
            new() {
                sliderName   = "ImportanceWeight5",
                labelName    = "WeightValue5",
                defaultValue = 1f,
                defaultText  = "1.0",
                stepSize     = FloatStepSize,
                format       = FloatFormat
            },

            // Finger Tone
            new() {
                sliderName   = "OuterOffset",
                labelName    = "OuterOffsetValue",
                defaultValue = 1f,
                defaultText  = "1.0",
                stepSize     = FloatStepSize,
                format       = FloatFormat
            },
            new() {
                sliderName   = "InnerOffset",
                labelName    = "InnerOffsetValue",
                defaultValue = 0.6f,
                defaultText  = "0.6",
                stepSize     = FloatStepSize,
                format       = FloatFormat
            },
        };
        
        public ParameterSettings() {
            
            var parameters     = Resources.Load<StyleSheet>("Styles/ParametersStyle");
            var parametersUxml = Resources.Load<VisualTreeAsset>("ParametersContainer");

            if (parameters != null) {
                styleSheets.Add(parameters);
                AddToClassList("custom-parameter-settings-container");
            }
            else {
                Debug.LogError("Failed to load StyleSheet: ParametersStyle.uss.");
            }

            if (parametersUxml != null) {
                parametersUxml.CloneTree(this);
            }
            else {
                Debug.LogError("Failed to load VisualTreeAsset: ParametersContainer.uxml");
            }
            
            foreach (var sp in sliderParams) {
                SetupSlider(sp.sliderName, sp.labelName, sp.stepSize, sp.format);
            }
            
            var defaultButton = this.Q<Button>("DefaultButton");
            defaultButton?.RegisterCallback<ClickEvent>(_ => RestoreDefaults());
        }

        
        private void SetupSlider(string sliderName,
            string labelName,
            float stepSize,
            string format) {
            var slider = this.Q<Slider>(sliderName);
            var label  = this.Q<Label>(labelName);

            if (slider == null) return;
            slider.RegisterValueChangedCallback(evt => {
                var newValue = SnapToStep(evt.newValue, stepSize);
                slider.SetValueWithoutNotify(newValue);
            });
                
            if (label != null) {
                slider.RegisterValueChangedCallback(evt => {
                    label.text = evt.newValue.ToString(format);
                });
            }
        }

        private void RestoreDefaults() {
            foreach (var sp in sliderParams) {
                var slider = this.Q<Slider>(sp.sliderName);
                var label = this.Q<Label>(sp.labelName);
                slider.value = sp.defaultValue;
                label.text = sp.defaultText;
            }
        }
        
        private float SnapToStep(float input, float step) {
            return Mathf.Round(input / step) * step;
        }
    }
}