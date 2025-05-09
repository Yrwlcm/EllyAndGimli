using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [SerializeField] private float _fadeDuration = 0.5f;
    private bool _isReloading;
    private InputSystem_Actions _inputSystemActions;


    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        
        _inputSystemActions = new InputSystem_Actions();
        _inputSystemActions.UI.Enable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _isReloading = false;
        StartCoroutine(ScreenFader.Instance.FadeIn(_fadeDuration));
    }

    private void Update()
    {
        if (!_isReloading &&
            SceneManager.GetActiveScene().name != "MainMenu" &&
            _inputSystemActions.UI.Menu.IsPressed()) 
        {
            LoadNextScene("MainMenu");
        }
    }

    public void RestartLevel()
    {
        if (_isReloading) return;
        _isReloading = true;
        StartCoroutine(LoadRoutine(SceneManager.GetActiveScene().buildIndex));
    }

    public void LoadNextScene(string sceneName)
    {
        if (_isReloading) return;
        _isReloading = true;
        StartCoroutine(LoadRoutine(sceneName));
    }
    
    private IEnumerator LoadRoutine(object target)
    {
        yield return ScreenFader.Instance.FadeOut(_fadeDuration);

        var op = (target is int build)
            ? SceneManager.LoadSceneAsync(build)
            : SceneManager.LoadSceneAsync(target as string);

        while (!op.isDone) yield return null;
    }
}
