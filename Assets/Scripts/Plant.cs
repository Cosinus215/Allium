using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[CreateAssetMenu(menuName = "Plants/base plant", fileName = "NewPlantData")]
public class Plant : ScriptableObject
{
    //Potencjalne problemy do omowienia:
    //>Co z grafikami? Kopiujac zajmujemy wiecej pamieci? Moze klasa PlantData
    //przechowujaca je w jednym miejscu caly czas? Ale jak ja wtedy zapisac? ID? NameID?
    //>Stan rosliny powinien byc w SO? Czy nie bedziemy tego naduzywac i wystarczy enum?

    [SerializeField] private string plantName = "default plant";
    [SerializeField] private Sprite plantIcon;
    [SerializeField] private Texture[] plantTextures;
    [Tooltip("How long plant sould growth in seconds")]
    [SerializeField] private int plantGrowthTime = 20;
    [Tooltip("Current plant age in seconds")]
    [SerializeField] private int plantAge = 0;
    public enum PlantState {Growing,ReadyToHarvest};
    private PlantState currentPlantState;

    private int growthStage = -1;

    public string GetPlantName()
    {
        return plantName;
    }
    public Sprite GetPlantIcon()
    {
        return plantIcon;
    }
    public Texture GetPlantTexture()
    {
        return plantTextures[CurrentTextureID()];
    }
    public void UpdateGraphic(Renderer target)
    {
        if(growthStage != CurrentTextureID())
        {
            growthStage = CurrentTextureID();
            target.material.SetTexture("_BaseMap", plantTextures[growthStage]);
        }
    }
    public void ResetPlant()
    {
        currentPlantState = PlantState.Growing;
        growthStage = -1;
        plantAge = 0;
    }
    private int CurrentTextureID()
    {
        int stages = plantTextures.Length;
        if (stages == 0)
        {
            return 0;
        }
        int id = 0;
        if (currentPlantState == PlantState.ReadyToHarvest || plantGrowthTime == 0 )
        {
            //Plant if full grown
            id = stages - 1;
        }
        else
        {
            id = (plantAge / (plantGrowthTime / (stages - 1)));
            if (id >= stages - 1)
            {
                //plant isnt full grown
                id = stages - 2;
            }
        }
        
        id = Mathf.Clamp(id, 0, stages - 1);
        return id;
    }

    public void OnTick()
    {
        plantAge++;
        if(plantAge > plantGrowthTime)
        {
            plantAge = plantGrowthTime;
            if (currentPlantState == PlantState.Growing)
            {
                currentPlantState = PlantState.ReadyToHarvest;
                Debug.Log($"{plantName} is ready to harvest!");
            }
        }
    }
}