using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Components.SettingsPage {
    public class AudioSettings : VisualElement
    {
        public AudioSettings() {
            var audioSettingsStyle = Resources.Load<StyleSheet>("Styles/AudioSettingsStyle");
            var audioSettingsUxml = Resources.Load<VisualTreeAsset>("SoundSettingsContainer");
            if (audioSettingsStyle != null) {
                styleSheets.Add(audioSettingsStyle);
            }
            if (audioSettingsUxml != null) {
                audioSettingsUxml.CloneTree(this);
            }
            else {
                Debug.LogError(
                    "Failed to load StyleSheet: BoxPlotStyle.uss. Make sure it's placed in a Resources/Styles/ folder.");
            }
        }
    
    }
}
