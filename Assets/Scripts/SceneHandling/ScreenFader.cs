using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace SceneHandling {
    public class ScreenFader : MonoBehaviour {
        
        [SerializeField] private float _speed = 1.0f;
        [SerializeField, Range(0f, 1f)] private float _intensity = 0.0f;
        [SerializeField] private Color _color = Color.white;
        [SerializeField] private Camera _vrCamera = null;
        [SerializeField] private string _layerName = "Persistent";

        private Image _fadeImage;

        private void Awake() {
            // Create a full-screen UI Image
            var canvasObject = new GameObject("ScreenFaderCanvas");
            canvasObject.transform.SetParent(transform, false);

            var canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = _vrCamera; // Assign the VR camera
            canvas.planeDistance = 0.5f; // Adjust as needed

            var canvasScaler = canvasObject.AddComponent<CanvasScaler>();
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.referenceResolution = new Vector2(1920, 1080); // Adjust as needed

            // Create the Image
            var imageObject = new GameObject("FadeImage");
            imageObject.transform.SetParent(canvasObject.transform, false);

            _fadeImage = imageObject.AddComponent<Image>();
            _fadeImage.color = new Color(_color.r, _color.g, _color.b, _intensity);
            _fadeImage.rectTransform.anchorMin = Vector2.zero;
            _fadeImage.rectTransform.anchorMax = Vector2.one;
            _fadeImage.rectTransform.offsetMin = Vector2.zero;
            _fadeImage.rectTransform.offsetMax = Vector2.zero;
            // Set the layer on the canvas and its children
            var layer = LayerMask.NameToLayer(_layerName);
            if (layer == -1) {
                Debug.LogError($"Layer '{_layerName}' not found. Please add it to the Tags and Layers manager.");
            }
            else {
                SetLayerRecursively(canvasObject, layer);
            }
        }

        private void SetLayerRecursively(GameObject obj, int newLayer) {
            if (obj == null) return;

            obj.layer = newLayer;

            foreach (Transform child in obj.transform) {
                if (child == null) continue;
                SetLayerRecursively(child.gameObject, newLayer);
            }
        }

        public Coroutine StartFadeIn() {
            StopAllCoroutines();
            return StartCoroutine(FadeIn());
        }

        private IEnumerator FadeIn() {
            while (_intensity < 1.0f) {
                _intensity += _speed * Time.deltaTime;
                _intensity = Mathf.Clamp01(_intensity);
                UpdateFadeImage();
                yield return null;
            }
        }

        public Coroutine StartFadeOut() {
            StopAllCoroutines();
            return StartCoroutine(FadeOut());
        }

        private IEnumerator FadeOut() {
            while (_intensity > 0.0f) {
                _intensity -= _speed * Time.deltaTime;
                _intensity = Mathf.Clamp01(_intensity);
                UpdateFadeImage();
                yield return null;
            }
        }

        private void UpdateFadeImage() {
            if (_fadeImage != null) {
                _fadeImage.color = new Color(_color.r, _color.g, _color.b, _intensity);
            }
        }
    }
}