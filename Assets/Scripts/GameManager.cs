using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [Header("Overall")]
    public const int SECONDS_PER_TICK = 1;
    private WaitForSeconds tickTime = new WaitForSeconds(SECONDS_PER_TICK);
    [SerializeField] private GameEvent tickEvent;

    [Header("Time")]
    [SerializeField, Range(0,24)] private byte dayTick = 0;
    private ulong gameTick = 0;
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
            dayTick = (byte)System.DateTime.Now.Hour;

            if(gameTick >= weatherUpdateTick)
            {
                ChangeWeather();
            }

            HandleDayCycle();

            tickEvent.Raise();
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
        if (!preset || !sun) return;

        RenderSettings.ambientLight = preset.ambientColor.Evaluate(dayPercentage);
        RenderSettings.fogColor = preset.fogColor.Evaluate(dayPercentage);

        sun.color = preset.directionalColor.Evaluate(dayPercentage);
        sun.transform.localRotation = Quaternion.Euler(
            new Vector3(sunAngleOverTime.Evaluate(dayPercentage)*180,
            -170,
            0));
    }
    private void ChangeWeather()
    {
        weatherUpdateTick += (uint)UnityEngine.Random.Range(
            WEATHER_MIN_UPDATE_TRESHOLD,
            WEATHER_MAX_UPDATE_TRESHOLD);

        int newWeatherID = UnityEngine.Random.Range(0, weather.Length);

        currentWeather.ToogleWeather(false);
        for (int i=0;i<weather.Length;i++)
        {
            if (i == newWeatherID)
            {
                currentWeather = weather[i];
                currentWeather.ToogleWeather(true);
                return;
            }
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
