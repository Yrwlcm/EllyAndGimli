using System;
using UnityEngine;
using UnityEngine.Events;

public class Crystal : MonoBehaviour
{
	private string _requiredColliderTag;
	private readonly UnityEvent _onCrystalCollected = new();

	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag(_requiredColliderTag))
		{
			_onCrystalCollected?.Invoke();
			Destroy(gameObject);
		}
	}

	public void AddListenerForTag(UnityAction unityAction, string tag)
	{
		_onCrystalCollected.AddListener(unityAction);
		_requiredColliderTag = tag;
	}
}
