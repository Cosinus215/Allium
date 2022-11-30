using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Overall")]
    public const int SECONDS_PER_TICK = 1;
    private WaitForSeconds tickTime = new WaitForSeconds(SECONDS_PER_TICK);
    [SerializeField] private GameEvent tickEvent;

    [Header("Time")]
    [SerializeField] private int dayTime = 0;
    private int ticksPerDay  = 24;
    private int sunriseTick = 6;
    private int sunsetTick = 18;
    private uint gameTick = 0;
    [Space]
    [SerializeField] private float currentSunAngle = 0;
    [SerializeField] private float sunriseSunAngle = 90;
    [SerializeField] private float sunsetSunAngle = 180;
    [Space]
    [SerializeField] private Transform sun;
    public enum Weather { Sunny, Cloudy, Rainy };
    [Header("Weather")]
    public Weather currentWeather;

    private void Start()
    {
        //Internal game timer
        StartCoroutine(GameTick());
    }

    private IEnumerator GameTick()
    {
        for (;;)
        {
            ++gameTick;
            tickEvent.Raise();
            HandleDayCycle();
            yield return tickTime;
        }
    }
    private void HandleDayCycle()
    {
        dayTime++;
        if (dayTime > ticksPerDay)
        {
            dayTime = 0;
        }
        if (dayTime >= sunriseTick && dayTime <= sunsetTick)
        {
            //Day
            float sunPercentage = (float)((float)dayTime - (float)sunriseTick) / (float)((float)sunsetTick - (float)sunriseTick);
            currentSunAngle = Mathf.Lerp(sunriseSunAngle, sunsetSunAngle, sunPercentage);
            //Debug.Log($"Day: {sunPercentage*100}%");
        }
        else
        {
            //Night
            float nightLength = ticksPerDay - sunsetTick + sunriseTick;
            float nightTick = 0;
            if (dayTime < sunriseTick)
            {
                nightTick = (ticksPerDay - sunsetTick) + dayTime + 1;
            }
            else
            {
                nightTick = Mathf.Abs(dayTime - sunsetTick);
            }
            float nightPercentage = nightTick / nightLength;
            currentSunAngle = Mathf.Lerp(sunsetSunAngle, sunriseSunAngle, nightPercentage);
            //Debug.Log($"Night: {nightPercentage*100}% {nightTick} z {nightLength}");
        }

        if (sun)
        {
            sun.rotation = new Quaternion(currentSunAngle, 0, 0, 0);
        }
        
    }
    private void ChangeWeather()
    {
        
    }
    public Weather GetCurrentWeather()
    {
        return currentWeather;
    }
}
