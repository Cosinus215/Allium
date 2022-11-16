using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[CreateAssetMenu(menuName = "Plants/base plant", fileName = "NewPlantData")]
public class Plant : ScriptableObject
{
    [SerializeField] private string plantName = "default plant";
    [SerializeField] private Sprite plantIcon;
    [Tooltip("How long plant sould growth in seconds")]
    [SerializeField] private int plantGrowthTime = 10;
    private int plantAge = 0; //in seconds
    public enum PlantState {Growing,ReadyToHarvest};
    private PlantState currentPlantState;
    public string GetPlantName()
    {
        return plantName;
    }
    public Sprite GetPlantIcon()
    {
        return plantIcon;
    }
    public void OnTick()
    {
        plantAge++;
        if(plantAge > plantGrowthTime)
        {
            plantAge = plantGrowthTime;
            currentPlantState = PlantState.ReadyToHarvest;
            Debug.Log($"{plantName} is ready to harvest! {currentPlantState}");
        }
    }
}
