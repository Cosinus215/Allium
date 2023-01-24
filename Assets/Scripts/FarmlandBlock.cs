using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//https://learn.unity.com/tutorial/interfaces#
public class FarmlandBlock : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject plantRoot;
    private Renderer plantMaterial;
    private Plant currentPlant;
    private int waterLevel = 0;

    private void Awake()
    {
        if(plantRoot                  is not                              null)
        {
            plantMaterial = plantRoot.GetComponentInChildren<Renderer>();
        }
    }

    public bool SetPlantData(PlantData newPlantData) 
    {
        if(newPlantData != null) 
        {
            if (currentPlant == null) {
                currentPlant = new Plant(newPlantData);
                currentPlant.UpdateGraphic(plantMaterial);
                plantRoot.SetActive(true);
                return true;
            } else {
                return false;
            }
        }
        else
        {
            plantRoot.SetActive(false);
            currentPlant = null;
            return false;
        }
    }
    public void WaterBlock(int additionalWater = 10)
    {
        waterLevel += additionalWater;
        waterLevel = Mathf.Clamp(waterLevel,0,101);
    }
    public void OnTick(int t)
    {
        if (t>0)
        {
            if (GameManager.Instance.currentWeather.isWatering)
            {
                WaterBlock(10);
            }
            else
            {
                WaterBlock(-1);
            }
        }
        

        if (currentPlant != null && waterLevel >= 0)
        {
            currentPlant.OnTick(plantMaterial, t);
        }

        SetPlantColor();
    }

    private void SetPlantColor() {
        if (currentPlant == null) {
            return;
        }

        if (waterLevel == 0) {
            plantMaterial.material.color = Color.yellow;
        } else {
            plantMaterial.material.color = Color.white;
        }
    }

    public bool Interact(Items item = null)
    {
        //Debug.Log($"Interaction with: {name}");
        switch (item.itemType) 
        {
            case Items.Type.Axe:
                Debug.Log("Ciupaga");
                return false;

            case Items.Type.Hoe:
                return false;

            case Items.Type.Shovel:
                Debug.Log("Lopata");
                return false;

            case Items.Type.Can:
                WaterBlock(25);
                return true;

            case Items.Type.Bundle:
                seed seedling = Inventory_System.Instance.GetSeed();

                if (seedling != null && seedling.number > 0) {
                    if (SetPlantData(seedling.plant)) {
                        seedling.number--;

                        if (seedling.number <= 0) {
                            Inventory_System.Instance.Bundle_Inv
                                .RemoveAt(Inventory_System.Instance.SeedNumber);
                        }
                        return true;
                    } else {
                        return false;
                    }
                } else {
                    return false;
                }

            default:
                Debug.Log("Shitty item");
                return false;
        }
    }
    public Plant GetCurrentPlant()
    {
        return currentPlant;
    }
    public int GetWaterLevel()
    {
        return waterLevel;
    }
    public void SetFarmlandBlock(FarmlandBlockSaveData fbsd)
    {
        if(fbsd.plantID != -1)
        {
            SetPlantData(GameManager.Instance.GetPlantDataByID(fbsd.plantID));
            currentPlant.SetPlantAge(fbsd.plantAge);
        }
        else
        {
            SetPlantData(null);
        }
        waterLevel = fbsd.waterLevel;
    }
}
