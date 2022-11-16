using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Czy potrzebujemy interfejsu "interakcja"? Farm block moze go implementowac
//Potrzebujemy klasy nadrzednej "Block"? Czy beda inne bloki?
public class FarmlandBlock : MonoBehaviour
{
    [SerializeField] private GameObject plantRoot;
    private Renderer plantMaterial;
    [SerializeField] private Plant plantData;

    private void Awake()
    {
        if(plantRoot != null)
        {
            plantMaterial = plantRoot.GetComponentInChildren<Renderer>();
        }
        SetPlantData(plantData);
    }

    public void SetPlantData(Plant newPlant) 
    {
        if(plantData != null) 
        {
            plantData = Instantiate(newPlant);
            plantData.name = $"{newPlant.GetPlantName()} clone";
            plantData.ResetPlant();
            plantData.UpdateGraphic(plantMaterial);
            //plantMaterial.material.SetTexture("_BaseMap", plantData.GetPlantTexture());
            plantRoot.SetActive(true); 
        }
        else
        {
            plantRoot.SetActive(false);
            Destroy(plantData);
            plantData = null;   
        }
    }

    public void OnTick()
    {
        if(plantData != null)
        {
            plantData.OnTick();
            plantData.UpdateGraphic(plantMaterial);
            //plantMaterial.material.SetTexture("_BaseMap", plantData.GetPlantTexture());
        }
    }
}
