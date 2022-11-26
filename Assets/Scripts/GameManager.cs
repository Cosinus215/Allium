using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public const int SECONDS_PER_TICK = 1;
    private WaitForSeconds tickTime = new WaitForSeconds(SECONDS_PER_TICK);
    [SerializeField] private GameEvent tickEvent;
    private void Start()
    {
        //Uruchamiamy petle ktora bedzie wewnetrznym zegarem naszej gry
        StartCoroutine(GameTick());
    }
    //Petla uruchamia event so
    //potem mozna dodac wiecej logiki
    //Kurtyny mozesz traktowac (tylko do pewnego stopnia i uproszczone) jako watki
    //Mozemy zrzucac tutaj akcje ktore dziac sie beda przez jakis czas
    //Lub jakies ciezkie i wolne obliczenia, nie blokujac watku gry (nie ma zacinek)
    private IEnumerator GameTick()
    {
        for (;;)//niech ci bedzie ze for ;)
        {
            tickEvent.Raise();
            yield return tickTime;
        }
    }
}
