using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameEvent : ScriptableObject
{
	private List<GameEventManager> listeners =
		new List<GameEventManager>();

	public void Raise()
	{
		for (int i = listeners.Count - 1; i >= 0; i--)
			listeners[i].OnEventRaised();
	}

	public void RegisterListener(GameEventManager listener)
	{ listeners.Add(listener); }

	public void UnregisterListener(GameEventManager listener)
	{ listeners.Remove(listener); }
}
