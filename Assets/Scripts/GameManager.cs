using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using NUnit.Framework.Constraints;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [Header("Overall")]
    public const int SECONDS_PER_TICK = 1;
    private WaitForSeconds tickTime = new WaitForSeconds(SECONDS_PER_TICK);
    [SerializeField] private GameEvent tickEvent;
    [SerializeField] private PlantList plantsList;

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

    public SaveFile sv;
    public string path;
    private void Start()
    {
        if(Instance != null)
        {
            Debug.LogWarning("Multiple managers!");
            Debug.Log(Instance.gameObject);
        }
        else
        {
            Instance = this;
        }
        path = Application.persistentDataPath + "/save.fly";
        //Internal game timer
        StartCoroutine(GameTick());
    }
    private IEnumerator GameTick()
    {
        for (;;)
        {
            ++gameTick;
            dayTick = (byte)System.DateTime.Now.Hour;
            //dayTick = 8;

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
    public PlantData GetPlantDataByID(int id)
    {
        if(plantsList != null & plantsList.Plants.Count > id)
        {
            return plantsList.Plants[id];
        }
        return null;
    }
    [ContextMenu("Save game")]
    public void SaveGame()
    {
        List<Plant> plants = new List<Plant>();
        foreach(FarmlandBlock fb in FindObjectsOfType<FarmlandBlock>())
        {
            Plant p = fb.GetCurrentPlant();
            if (p!=null)
            {
                plants.Add(p);
            }
        }

        sv = new SaveFile(plants);
        
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(path, FileMode.CreateNew);
        Debug.LogWarning(path);
        bf.Serialize(file, sv);
        file.Close();
    }
    [ContextMenu("Load game")]
    public void LoadGame()
    {
        if (!File.Exists(path))
        { 
            Debug.Log("No savefile!"); 
            return;
        }
        FileStream plik = File.Open(path, FileMode.Open);

        if (plik.Length <= 0)
        {
            Debug.Log("Savefile empty!");
            return;
        }
    
        BinaryFormatter bf = new BinaryFormatter();
        try
        {
            sv = (SaveFile)bf.Deserialize(plik);           
        }
        catch
        {
            Debug.LogError("File is damaged!");
            plik.Close();
            return;
        }
        if (sv != null)
        {
            Inventory_System.Instance.ClearSeedEq();
            foreach (SeedSaveData ssd in sv.seeds)
            {
                Inventory_System.Instance.AddSeedToInv(new seed(ssd));
            }
        }
        else
        {
            Debug.LogError("Class is damaged!");
        }

        plik.Close();
    }

    //Metoda wykonywana nawet w edytorze - mo�emy mie� podgl�d nawet poza gr�!
    public void OnValidate()
    {
        HandleDayCycle();
    }
}
