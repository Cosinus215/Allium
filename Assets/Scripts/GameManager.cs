using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [Header("Overall")]
    public const int SECONDS_PER_TICK = 1;
    private WaitForSeconds tickTime = new WaitForSeconds(SECONDS_PER_TICK);
    [SerializeField] private GameEvent tickEvent;

    [Header("Time")]
    [SerializeField, Range(0,24)] private int dayTick = 0;
    private uint gameTick = 0;
    [Space]
    [SerializeField] private Light sun;
    [SerializeField] private AnimationCurve sunAngleOverTime;

    [Header("Weather")]
    public Weather currentWeather;
    public Weather[] weather;
    private uint weatherUpdateTick = 0;

    private const uint WEATHER_MIN_UPDATE_TRESHOLD = 5;
    private const uint WEATHER_MAX_UPDATE_TRESHOLD = 10;
    private void Start()
    {
        if(Instance != null)
        {
            Debug.LogWarning("Multiple managers!");
        }
        else
        {
            Instance = this;
        }
        //Internal game timer
        StartCoroutine(GameTick());
    }

    private IEnumerator GameTick()
    {
        for (;;)
        {
            ++gameTick;
            ++dayTick;
            if (dayTick >= 24)
            {
                dayTick = 0;
            }
            tickEvent.Raise();
            HandleDayCycle();

            if(gameTick >= weatherUpdateTick)
            {
                ChangeWeather();
            }
            yield return tickTime;
        }
    }
    private void HandleDayCycle()
    {
        /*
         * Later move to some sort of coroutine to avoid "blockines" of
         * light updates
        */
        UpdateLighting((float)dayTick/ 24);
    }
    private void UpdateLighting(float dayPercentage)
    {
        LightPreset preset = currentWeather.lightPreset;
        if (!preset || ! sun) return;

        RenderSettings.ambientLight = preset.ambientColor.Evaluate(dayPercentage);
        RenderSettings.fogColor = preset.fogColor.Evaluate(dayPercentage);

        sun.color = preset.directionalColor.Evaluate(dayPercentage);
        sun.transform.localRotation = Quaternion.Euler(
            new Vector3(sunAngleOverTime.Evaluate(dayPercentage)*180,
            -170,
            0));

        sun.transform.localRotation = Quaternion.Euler(new Vector3((dayPercentage*360f)-90f,-170,0));
    }
    private void ChangeWeather()
    {
        weatherUpdateTick += (uint)UnityEngine.Random.Range(
            WEATHER_MIN_UPDATE_TRESHOLD,
            WEATHER_MAX_UPDATE_TRESHOLD);
        int newWeatherID = UnityEngine.Random.Range(0, weather.Length);

        for (int i=0;i<weather.Count();i++)
        {
            if (i == newWeatherID)
            {
                currentWeather = weather[i];
            }
            weather[i].ToogleWeater(i == newWeatherID);
        }
    }
    public Weather GetCurrentWeather()
    {
        return currentWeather;
    }
    //Metoda wykonywana nawet w edytorze - mo¿emy mieæ podgl¹d nawet poza gr¹!
    public void OnValidate()
    {
        HandleDayCycle();
    }
}
