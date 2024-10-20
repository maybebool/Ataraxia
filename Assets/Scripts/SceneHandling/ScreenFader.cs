using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace SceneHandling {
    public class ScreenFader : MonoBehaviour
    {
    //     [SerializeField] private float _speed = 1.0f;
    //     [SerializeField] private float _intensity = 0.0f;
    //     [SerializeField] private Color _color = Color.black;
    //     [SerializeField] private Material _fadeMaterial = null;
    //
    //     private void OnRenderImage(RenderTexture source, RenderTexture destination)
    //     {
    //         _fadeMaterial.SetFloat("_Intensity", _intensity);
    //         _fadeMaterial.SetColor("_FadeColor", _color);
    //         Graphics.Blit(source, destination, _fadeMaterial);
    //     }
    //
    //     public Coroutine StartFadeIn()
    //     {
    //         StopAllCoroutines();
    //         return StartCoroutine(FadeIn());
    //     }
    //
    //     private IEnumerator FadeIn()
    //     {
    //         while (_intensity <= 1.0f)
    //         {
    //             _intensity += _speed * Time.deltaTime;
    //             yield return null;
    //         }
    //     }
    //
    //     public Coroutine StartFadeOut()
    //     {
    //         StopAllCoroutines();
    //         return StartCoroutine(FadeOut());
    //     }
    //
    //     private IEnumerator FadeOut()
    //     {
    //         while (_intensity >= 0.0f)
    //         {
    //             _intensity -= _speed * Time.deltaTime;
    //             yield return null;
    //         }
    //     }
    // }
    
        [SerializeField] private float _speed = 1.0f;
        [SerializeField, Range(0f, 1f)] private float _intensity = 0.0f;
        [SerializeField] private Color _color = Color.white;

        private Image _fadeImage;

        private void Awake()
        {
            // Create a full-screen UI Image
            GameObject canvasObject = new GameObject("ScreenFaderCanvas");
            canvasObject.transform.SetParent(transform, false);

            Canvas canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = short.MaxValue; // Ensure it's on top

            CanvasScaler canvasScaler = canvasObject.AddComponent<CanvasScaler>();
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;

            // Create the Image
            GameObject imageObject = new GameObject("FadeImage");
            imageObject.transform.SetParent(canvasObject.transform, false);

            _fadeImage = imageObject.AddComponent<Image>();
            _fadeImage.color = new Color(_color.r, _color.g, _color.b, _intensity);
            _fadeImage.rectTransform.anchorMin = Vector2.zero;
            _fadeImage.rectTransform.anchorMax = Vector2.one;
            _fadeImage.rectTransform.offsetMin = Vector2.zero;
            _fadeImage.rectTransform.offsetMax = Vector2.one;
        }

        public Coroutine StartFadeIn()
        {
            StopAllCoroutines();
            return StartCoroutine(FadeIn());
        }

        private IEnumerator FadeIn()
        {
            while (_intensity < 1.0f)
            {
                _intensity += _speed * Time.deltaTime;
                _intensity = Mathf.Clamp01(_intensity);
                UpdateFadeImage();
                yield return null;
            }
        }

        public Coroutine StartFadeOut()
        {
            StopAllCoroutines();
            return StartCoroutine(FadeOut());
        }

        private IEnumerator FadeOut()
        {
            while (_intensity > 0.0f)
            {
                _intensity -= _speed * Time.deltaTime;
                _intensity = Mathf.Clamp01(_intensity);
                UpdateFadeImage();
                yield return null;
            }
        }

        private void UpdateFadeImage()
        {
            if (_fadeImage != null)
            {
                _fadeImage.color = new Color(_color.r, _color.g, _color.b, _intensity);
            }
        }
    }
}
