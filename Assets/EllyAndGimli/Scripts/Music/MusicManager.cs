using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager _instance;
	private AudioSource _audioSource;
	private bool _isOff = false;

	private void Awake()
	{
		if (_instance == null)
		{
			_instance = this;
			_audioSource = gameObject.GetComponent<AudioSource>();
			DontDestroyOnLoad(gameObject);
			return;
		}
		Destroy(gameObject);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.E))
		{
			_isOff = !_isOff;
			_audioSource.volume = _isOff ? 0.0f : 0.15f;
		}
	}
}
