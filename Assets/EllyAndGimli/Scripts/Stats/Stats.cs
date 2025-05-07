using UnityEngine;

public class Stats : MonoBehaviour
{
	[SerializeField] private PlayerControllerBase _elly;
	[SerializeField] private PlayerControllerBase _gimli;

	public int EllyDeathCounter { get; private set; }
	public int GimliDeathCounter { get; private set; }

	private void Start()
	{
		_elly.OnDead += () => EllyDeathCounter++;
		_gimli.OnDead += () => GimliDeathCounter++;
	}
}
