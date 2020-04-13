using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class EventTransform : UnityEvent<Transform> { }

public class GameEventManagerTransform : MonoBehaviour
{
    public GameEventTransform Event;
    public EventTransform Response;

    private void OnEnable()
    { Event.RegisterListener(this); }

    private void OnDisable()
    { Event.UnregisterListener(this); }

    public void OnEventRaised(Transform n)
    { Response.Invoke(n); }
}
