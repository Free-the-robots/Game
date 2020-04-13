using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameEventTransform : ScriptableObject
{
	private List<GameEventManagerTransform> listeners =
		new List<GameEventManagerTransform>();

	public void Raise(Transform n)
	{
		for (int i = listeners.Count - 1; i >= 0; i--)
			listeners[i].OnEventRaised(n);
	}

	public void RegisterListener(GameEventManagerTransform listener)
	{ listeners.Add(listener); }

	public void UnregisterListener(GameEventManagerTransform listener)
	{ listeners.Remove(listener); }
}