using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
//Wiem ze nie lubisz podpinac graficznie
//Moge to przeniesc jako interfejs czy cos
//Zadaniemskryptu jest zapisanie sie do eventu
//zeby ten wiedzial do kogo sie zglosic kiedy zostanie wykonany
public class GameEventListener : MonoBehaviour
{
    public GameEvent Event;
    public UnityEvent Response;

    private void OnEnable()
    { 
        Event.RegisterListener(this); 
    }

    private void OnDisable()
    { 
        Event.UnregisterListener(this); 
    }

    public void OnEventRaised()
    { 
        Response.Invoke(); 
    }
}
