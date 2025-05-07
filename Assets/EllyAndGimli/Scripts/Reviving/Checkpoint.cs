using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
	private readonly HashSet<PlayerControllerBase> _enteredEntities = new();

	public delegate void CheckpointEntered(PlayerControllerBase entity);
	public event CheckpointEntered OnCheckpointEntered;

	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (!collision.TryGetComponent<PlayerControllerBase>(out var player)) return;
		var wasPresented = _enteredEntities.Contains(player);
		if (wasPresented) return;
		_enteredEntities.Add(player);
		OnCheckpointEntered(player);
	}
}
