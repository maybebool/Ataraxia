using System;
using Audio;
using SceneHandling;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace GameUI {
    public class TestUIController : MonoBehaviour
    {
        [SerializeField] private Button exercise1;
        [SerializeField] private Button exercise2;
        [SerializeField] private Button exercise3;
        [SerializeField] private Button exercise4;

        private void OnEnable() {
            UIUtil.CallMultipleActions(exercise1, ()=> OnClickStartASceneButton(SceneNames.Exercise1));
            UIUtil.CallMultipleActions(exercise2, ()=> OnClickStartASceneButton(SceneNames.Exercise2));
            UIUtil.CallMultipleActions(exercise3, ()=> OnClickStartASceneButton(SceneNames.Exercise3));
            UIUtil.CallMultipleActions(exercise4, ()=> OnClickStartASceneButton(SceneNames.Exercise4));
        }

        private void OnDisable() {
            exercise1.onClick.RemoveAllListeners();
            exercise2.onClick.RemoveAllListeners();
            exercise3.onClick.RemoveAllListeners();
            exercise4.onClick.RemoveAllListeners();
        }


        private void OnClickStartASceneButton(SceneNames scene) {
            SceneLoader.Instance.LoadNewScene(scene);
        }
    
    }
}
