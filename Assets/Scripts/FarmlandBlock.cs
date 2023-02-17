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
    [SerializeField] private Color healthColor;
    [SerializeField] private Color dryColor;

    private void Awake()
    {
        if (plantRoot                  is not                              null)
        {
            plantMaterial = plantRoot.GetComponentInChildren<Renderer>();
            plantMaterial.material.EnableKeyword("_EMISSION");
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
            plantMaterial.material.SetColor("_EmissionColor", dryColor);
        } else {
            plantMaterial.material.SetColor("_EmissionColor", healthColor);
        }
    }

    public bool Interact(Items item = null)
    {
        switch (item.itemType) 
        {
            case Items.Type.Axe:
                return false;

            case Items.Type.Hoe:
                if (currentPlant != null && currentPlant.IsReadyToHarvest())
                {
                    Inventory_System.Instance.AddMoney(currentPlant.GetPlantData().Price * 2);
                    SetPlantData(null);
                    return true;
                }
                return false;

            case Items.Type.Shovel:
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
