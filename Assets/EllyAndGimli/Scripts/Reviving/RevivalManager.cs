using System.Collections.Generic;
using UnityEngine;

public class RevivalManager : MonoBehaviour
{
    [SerializeField] private List<PlayerControllerBase> _revivableEntities = new();
	[SerializeField] private GameObject _checkpointsGameObject;

	private readonly Dictionary<string, Vector3> _revivalPoints = new();

	private void Start()
	{
		_revivableEntities.ForEach((e) =>
		{
			_revivalPoints.Add(e.tag, e.transform.position);
			e.OnRevival += () => RevivePlayerAtPosition(e);
		});
		var checkpointsCount = _checkpointsGameObject.transform.childCount;
		for (var i = 0; i < checkpointsCount; i++)
		{
			var child = _checkpointsGameObject.transform.GetChild(i);
			if (!child.gameObject.TryGetComponent<Checkpoint>(out var checkpoint)) continue;
			checkpoint.OnCheckpointEntered += OnCheckpointReached;
		}
	}

	private void RevivePlayerAtPosition(PlayerControllerBase player)
	{
		if (!_revivalPoints.ContainsKey(player.tag)) return;
		player.transform.position = _revivalPoints[player.tag];
	}

	private void OnCheckpointReached(PlayerControllerBase entity) => _revivalPoints[entity.tag] = entity.transform.position;
}
