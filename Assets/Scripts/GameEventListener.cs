using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    public GameEvent Event;
    public MyIntEvent Response;

    private void OnEnable()
    { 
        Event.RegisterListener(this); 
    }

    private void OnDisable()
    { 
        Event.UnregisterListener(this); 
    }

    public void OnEventRaised(int ticks)
    { 
        Response.Invoke(ticks); 
    }
}
[System.Serializable]
public class MyIntEvent : UnityEvent<int>
{
}