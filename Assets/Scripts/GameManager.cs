using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public const int SECONDS_PER_TICK = 1;
    private WaitForSeconds tickTime = new WaitForSeconds(SECONDS_PER_TICK);
    [SerializeField] private GameEvent tickEvent;

    public int dayTime = 0;
    public int ticksPerDay  = 24;
    public int sunriseTick = 6;
    public int sunsetTick = 18;

    public float currentSunAngle = 0;
    public float sunriseSunAngle = 90;
    public float sunsetSunAngle = 180;
    public enum Weather { Sunny, Cloudy, Rainy };
    public Weather currentWeather;

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
            dayTime++;
            if(dayTime > ticksPerDay)
            {
                dayTime = 0;
            }
            if (dayTime>=sunriseTick && dayTime<=sunsetTick)
            {
                //dzieñ
                float sunPercentage = (float)((float)dayTime - (float)sunriseTick) / (float)((float)sunsetTick - (float)sunriseTick);
                currentSunAngle = Mathf.Lerp(sunriseSunAngle,sunsetSunAngle, sunPercentage);
                Debug.Log($"Day: {sunPercentage}");
            }
            else
            {
                //noc
                /*
                float nightPercentage = 0;
                if (dayTime >= 0)
                {
                    nightPercentage = dayTime + ((float)ticksPerDay - sunsetTick) / (float)ticksPerDay - (sunsetTick + sunriseTick);
                }
                else
                {
                    nightPercentage = (float)ticksPerDay-dayTime / (float)ticksPerDay - (sunsetTick + sunriseTick);
                }

                currentSunAngle = Mathf.Lerp(sunsetSunAngle, sunriseSunAngle, nightPercentage);
                Debug.Log($"Night: {nightPercentage}");
                */
            }
            yield return tickTime;
        }
    }

    public Weather GetCurrentWeather()
    {
        return currentWeather;
    }
}
