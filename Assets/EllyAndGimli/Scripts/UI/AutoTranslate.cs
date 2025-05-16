using TMPro;
using UnityEngine;
using YG;

public class AutoTranslate : MonoBehaviour
{
    private TextMeshProUGUI _text;

    [SerializeField] private string _engVersion;

    private string _rusVersion;

    private void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _rusVersion = _text.text;
    }

	private void Update()
	{
		_text.text = YG2.envir.language.ToLower() switch
		{
			"ru" => _rusVersion,
			"en" => _engVersion,
			_ => _engVersion
		};
	}
}
