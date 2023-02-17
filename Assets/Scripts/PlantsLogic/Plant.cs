using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.InputSystem;

[System.Serializable]
public class Plant
{
    [SerializeField] private PlantData plantData;
    [SerializeField] private int plantAge = 0;
    public enum PlantState {Growing,ReadyToHarvest};
    private PlantState currentPlantState;

    private int growthStage = -1;
    public Plant(PlantData newData)
    {
        plantData = newData;
        ResetPlant();
    }
    public PlantData GetPlantData()
    {
        return plantData;
    }
    public void UpdateGraphic(Renderer target)
    {
        if (target == null || plantData==null)
        {
            return;
        }
        //zmieniamy grafike tylko gdy zachodzi zmiana :)
        //nie marnujmy mocy procesora na ladowanie grafiki bez potrzeby
        if (growthStage != CurrentTextureID())
        {
            growthStage = CurrentTextureID();
            target.material.SetTexture("_BaseMap", plantData.GetPlantTexture(growthStage));
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
        int stages = plantData.GrowthStages();
        if (stages == 0)
        {
            return 0;
        }
        int id = 0;
        if (currentPlantState == PlantState.ReadyToHarvest || plantData.GetPlantGrowthTime() == 0 )
        {
            //Roslinka w pelni wyrosnieta
            id = stages - 1;
        }
        else
        {
            id = (plantAge / (plantData.GetPlantGrowthTime() / (stages - 1)));
            if (id >= stages - 1)
            {
                //Roslinka prawie wyrosnieta
                id = stages - 2;
            }
        }
        //musimy sie upewnic ze nie wyjdziemy poza zakres tablicy
        id = Mathf.Clamp(id, 0, stages - 1);
        return id;
    }
    public void OnTick(Renderer targetGraphic, int time)
    {
        if(plantData == null)return;

        plantAge+= time;
        if(plantAge >= plantData.GetPlantGrowthTime())
        {
            plantAge = plantData.GetPlantGrowthTime();
            if (currentPlantState == PlantState.Growing)
            {
                currentPlantState = PlantState.ReadyToHarvest;
            }
        }
        UpdateGraphic(targetGraphic);
}
    public bool IsReadyToHarvest()
    {
        return currentPlantState== PlantState.ReadyToHarvest;
    }
    public int GetPlantAge()
    {
        return plantAge;
    }
    public void SetPlantAge(int age)
    {
        plantAge = age;
    }
}