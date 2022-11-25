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

    public void OnTick()
    {
        if(currentPlant != null)
        {
            currentPlant.OnTick(plantMaterial);
        }
    }

    public void Interact()
    {
        Debug.Log($"Interaction with: {name}");
    }
}
