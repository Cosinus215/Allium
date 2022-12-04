using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://learn.unity.com/tutorial/interfaces#
public class FarmlandBlock : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject plantRoot;
    private Renderer plantMaterial;
    [SerializeField] private PlantData plantDataSZABLON;
    [SerializeField] private Plant currentPlant;
    [SerializeField] private int waterLevel = 0;
    private void Awake()
    {
        if(plantRoot != null)
        {
            plantMaterial = plantRoot.GetComponentInChildren<Renderer>();
        }
        SetPlantData(plantDataSZABLON);//na potrzeby testow
        //potem octi bedzie sadzil lub GameManager po wczytaniu pliku
    }

    public void SetPlantData(PlantData newPlantData) 
    {
        if(newPlantData != null) 
        {
            currentPlant = new Plant(newPlantData);
            currentPlant.UpdateGraphic(plantMaterial);
            plantRoot.SetActive(true); 
        }
        else
        {
            plantRoot.SetActive(false);
            currentPlant = null;   
        }
    }
    public void WaterBlock(int additionalWater = 10)
    {
        waterLevel+=additionalWater;
        waterLevel = Mathf.Clamp(waterLevel,0,101);

    }
    public void OnTick()
    {
        if (GameManager.Instance.currentWeather.isWatering)
        {
            WaterBlock(10);
        }
        WaterBlock(-1);

        if (currentPlant != null && waterLevel>=0)
        {
            currentPlant.OnTick(plantMaterial);
        }
    }

    public void Interact()
    {
        Debug.Log($"Interaction with: {name}");
    }
}
