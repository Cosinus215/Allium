using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveFile
{
    public DateTime saveTime;
    public List<SeedSaveData> seeds;
    public List<FarmlandBlockSaveData> blocks;
    public int money;

    public SaveFile(List<FarmlandBlockSaveData> blocksData)
    {
        //czas
        saveTime = DateTime.Now;

        //nasiona
        seeds = new List<SeedSaveData>();
        foreach (seed s in Inventory_System.Instance.Bundle_Inv)
        {
            seeds.Add(new SeedSaveData(s));
        }

        //bloki
        blocks = blocksData;

        //pieni¹dze
        money = Inventory_System.Instance.GetMoneyAmout();
    }

}
[System.Serializable]
public class SeedSaveData
{
    public int amount;
    public int plantID;
    public SeedSaveData(seed mainPlant)
    {
        this.amount = mainPlant.number;
        this.plantID = mainPlant.plant.GetPlantID();
    }
}
[System.Serializable]
public class FarmlandBlockSaveData
{
    public int plantID;
    public int plantAge;
    public int waterLevel;
    public FarmlandBlockSaveData(FarmlandBlock fb)
    {
        Plant data = fb.GetCurrentPlant();
        if(data != null)
        {
            this.plantID = data.GetPlantData().GetPlantID();
            this.plantAge = fb.GetCurrentPlant().GetPlantAge();
            
        }
        else
        {
            this.plantID = -1;
            this.plantAge = 0;
        }
        this.waterLevel = fb.GetWaterLevel();
    }
}