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
    [SerializeField, Range(0,24)] private int dayTick = 0;
    private uint gameTick = 0;
    [Space]
    [SerializeField] private Light sun;
    [SerializeField] private LightPreset preset;
    public enum Weather { Sunny, Cloudy, Rainy };
    [Header("Weather")]
    public Weather currentWeather;
    private uint weatherUpdateTick = 0;
    private const uint WEATHER_MIN_UPDATE_TRESHOLD = 10;
    private const uint WEATHER_MAX_UPDATE_TRESHOLD = 30;
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
        if (!preset || ! sun) return;
        RenderSettings.ambientLight = preset.ambientColor.Evaluate(dayPercentage);
        RenderSettings.fogColor = preset.fogColor.Evaluate(dayPercentage);

        sun.color = preset.directionalColor.Evaluate(dayPercentage);
        sun.transform.localRotation = Quaternion.Euler(new Vector3((dayPercentage*360f)-90f,-170,0));
    }
    private void ChangeWeather()
    {
        weatherUpdateTick += (uint)UnityEngine.Random.Range(
            WEATHER_MIN_UPDATE_TRESHOLD,
            WEATHER_MAX_UPDATE_TRESHOLD);
        currentWeather = (Weather)UnityEngine.Random.Range(0, 3);
        Debug.Log($"Weather changed!");
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
