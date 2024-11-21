using System;
using System.Collections.Generic;
using Editor.Components.Buttons;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

namespace Editor.Components.CenterRowContainer {
    
    //[UxmlElement("CenterRowContainer")]
       
    public partial class CenterRowContainer : VisualElement{
        // public Buttons.UpperMainButton Button1 { get; private set; } = new UpperMainButton();
        // [HideInInspector] public Buttons.UpperMainButton button2 = new();
        // [HideInInspector] public readonly Buttons.UpperMainButton button3 = new();
        
        

        public CenterRowContainer(params UpperMainButton[] buttons) {
            var asset = Resources.Load<VisualTreeAsset>("CenterRowContainer");
            if (asset == null) {
                Debug.LogError("Failed to load VisualTreeAsset: CenterRowContainer");
                return;
            }
            var root = new VisualElement();

            // Clone the asset into the root VisualElement without creating an extra TemplateContainer
            asset.CloneTree(root);
            AddButtons(buttons);
            var centerRowContainerStyle = Resources.Load<StyleSheet>("Styles/CenterRowContainerStyle");
            if (centerRowContainerStyle != null) {
                styleSheets.Add(centerRowContainerStyle);
                AddToClassList("customContainerRow");
            }
            else {
                Debug.LogError("Failed to load StyleSheet: CenterRowContainerStyle.uss");
            }
        }

        private void AddButtons(UpperMainButton[] buttons) {
            foreach (var button in buttons) {
                Add(button);
            }
        }

        public CenterRowContainer(VisualElement element) {
        } 
    }
}