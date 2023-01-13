using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveFile
{
    //public DateTime saveTime;
    public List<SeedSaveData> seeds;
    //public List<Plant> plants;
    //public int money;

    public SaveFile(List<Plant> plantsToSave)
    {
        //saveTime = DateTime.Now;
        seeds = new List<SeedSaveData>();
        foreach (seed s in Inventory_System.Instance.Bundle_Inv)
        {
            seeds.Add(new SeedSaveData(s));
        }
        //plants = plantsToSave;
        //money= Inventory_System.Instance.GetMoneyAmout();
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