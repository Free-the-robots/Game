using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EventInt", menuName = "Event/EventInt", order = 1)]
public class GameEventInt : ScriptableObject
{
	private List<GameEventManagerInt> listeners =
		new List<GameEventManagerInt>();

	public void Raise(int n)
	{
		for (int i = listeners.Count - 1; i >= 0; i--)
			listeners[i].OnEventRaised(n);
	}

	public void RegisterListener(GameEventManagerInt listener)
	{ listeners.Add(listener); }

	public void UnregisterListener(GameEventManagerInt listener)
	{ listeners.Remove(listener); }
}
