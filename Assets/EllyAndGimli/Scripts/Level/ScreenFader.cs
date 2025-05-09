using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour
{
    public static ScreenFader Instance { get; private set; }

    [SerializeField] private Image _fadeImage;
    [SerializeField] private float _defaultDuration = 0.5f;
    [SerializeField] private Color _color = Color.black;
    
    private Canvas _canvas;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (_fadeImage == null)
            _fadeImage = GetComponentInChildren<Image>();
        
        _canvas = GetComponent<Canvas>();
        _fadeImage.color = new Color(_color.r, _color.g, _color.b, 0f);
        _fadeImage.raycastTarget = true;
    }

    public IEnumerator FadeOut(float duration = -1f) =>
        Fade(0f, 1f, duration < 0 ? _defaultDuration : duration, fadeOut: true);

    public IEnumerator FadeIn(float duration = -1f) =>
        Fade(1f, 0f, duration < 0 ? _defaultDuration : duration, fadeOut: false);

    public void InstantClear() =>
        _fadeImage.color = new Color(_color.r, _color.g, _color.b, 0f);
    
    private IEnumerator Fade(float from, float to, float duration, bool fadeOut)
    {
        if (fadeOut)
            _canvas.enabled = true;

        var t = 0f;
        var c = _color;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            var a = Mathf.Lerp(from, to, t / duration);
            _fadeImage.color = new Color(c.r, c.g, c.b, a);
            yield return null;
        }
        _fadeImage.color = new Color(c.r, c.g, c.b, to);
        
        if (!fadeOut)
            _canvas.enabled = false;
    }
}