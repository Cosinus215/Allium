using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private WaitForSeconds tickTime = new WaitForSeconds(1);
    [SerializeField] private GameEvent tickEvent;
    private void Start()
    {
        //Uruchamiamy petle ktora bedzie wewnetrznym zegarem naszej gry
        StartCoroutine(GameTick());
    }
    //Petla uruchamia event so
    //potem mozna dodac wiecej logiki
    private IEnumerator GameTick()
    {
        for (;;)//niech ci bedzie ;)
        {
            tickEvent.Raise();
            yield return tickTime;
        }
    }
}
