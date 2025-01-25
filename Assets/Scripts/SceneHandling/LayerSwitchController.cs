using UnityEngine;

namespace SceneHandling {
    public class LayerSwitchController : MonoBehaviour {
        public string targetLayer = "Persistent";
        private string _originalLayer = string.Empty;

        private void Awake() {
            _originalLayer = LayerMask.LayerToName(gameObject.layer);
        }

        private void OnEnable() {
            SceneLoader.Instance.onLoadBegin.AddListener(SwitchToLoadLayer);
            SceneLoader.Instance.onLoadEnd.AddListener(ResetLayer);
        }

        private void OnDisable() {
            SceneLoader.Instance.onLoadBegin.RemoveListener(SwitchToLoadLayer);
            SceneLoader.Instance.onLoadEnd.RemoveListener(ResetLayer);
        }

        private void SwitchToLoadLayer() {
            gameObject.layer = LayerMask.NameToLayer(targetLayer);
        }

        private void ResetLayer() {
            gameObject.layer = LayerMask.NameToLayer(_originalLayer);
        }
    }
}