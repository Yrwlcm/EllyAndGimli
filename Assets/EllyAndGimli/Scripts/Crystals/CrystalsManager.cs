using TMPro;
using UnityEngine;

public class CrystalsManager : MonoBehaviour
{
    public delegate void CrystalsCollected();
    public event CrystalsCollected OnCrystalsCollected;

	[SerializeField] private TextMeshProUGUI _ellyCrystalsText;
    [SerializeField] private TextMeshProUGUI _gimliCrystalsText;
    [SerializeField] private GameObject _crystals;
    [SerializeField] private LevelExit _levelExit;

    private readonly Color32 _collectedAllColor = new(70, 212, 21, 255);

    private int _ellyCrystalsToCollectCount;
    private int _gimliCrystalsToCollectCount;
    private int _ellyCrystalsCollectedCount;
    private int _gimliCrystalsCollectedCount;

    private void Start()
    {
        var childsCount = _crystals != null ? _crystals.transform.childCount : 0;
        for (var i = 0; i < childsCount; i++)
        {
            var child = _crystals.transform.GetChild(i);
            var crystalScript = child.gameObject.GetComponent<Crystal>();

			switch (child.tag)
            {
                case "EllyCrystal":
                    _ellyCrystalsToCollectCount++;
                    crystalScript.AddListenerForTag(EllyCrystalCollected, "Elly");
					break;
                case "GimliCrystal":
                    _gimliCrystalsToCollectCount++;
                    crystalScript.AddListenerForTag(GimliCrystalCollected, "Gimli");
                    break;
                default:
                    break;
            }
        }
        _ellyCrystalsText.text = $"0/{_ellyCrystalsToCollectCount}";
        _gimliCrystalsText.text = $"0/{_gimliCrystalsToCollectCount}";
        UpdateCrystalTextGUI(_ellyCrystalsText, _ellyCrystalsCollectedCount, _ellyCrystalsToCollectCount);
        UpdateCrystalTextGUI(_gimliCrystalsText, _gimliCrystalsCollectedCount, _gimliCrystalsToCollectCount);
		OnCrystalsCollected += _levelExit.OnAllCrystalsCollected;
	}

	private void Update()
	{
		if (_ellyCrystalsCollectedCount == _ellyCrystalsToCollectCount &&
            _gimliCrystalsCollectedCount == _gimliCrystalsToCollectCount)
        {
            OnCrystalsCollected();
            Destroy(gameObject);
        }
	}

	private void EllyCrystalCollected()
    {
        _ellyCrystalsCollectedCount++;
        UpdateCrystalTextGUI(_ellyCrystalsText, _ellyCrystalsCollectedCount, _ellyCrystalsToCollectCount);
	}

    private void GimliCrystalCollected()
    {
        _gimliCrystalsCollectedCount++;
        UpdateCrystalTextGUI(_gimliCrystalsText, _gimliCrystalsCollectedCount, _gimliCrystalsToCollectCount);

	}

    private void UpdateCrystalTextGUI(TextMeshProUGUI text, int collected, int toCollect)
    {
        text.text = $"{collected}/{toCollect}";
        if (collected == toCollect)
            text.color = _collectedAllColor;
    }
}
