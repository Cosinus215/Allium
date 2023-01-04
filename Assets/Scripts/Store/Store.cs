using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Store : MonoBehaviour {
    [SerializeField] private GameObject StoreUI;
    [SerializeField] private Button buttonPref;
    [SerializeField] private List<seed> magazyn = new List<seed>();

    private void OnTriggerEnter(Collider collider) {
        StoreUI.SetActive(true);
        UpdateStoreUI();
    }

    private void OnTriggerExit(Collider collider) {
        StoreUI.SetActive(false);
    }

    public void Buy(seed s) 
    {
        PlantData Seedinfo = s.plant;
        //czy mamy kasê
        int seed_inventory_index = FindSeedIndexInInv(Seedinfo);

        if (seed_inventory_index == -1) 
        {
            seed NewSeed = new seed();
            NewSeed.plant = Seedinfo;
            NewSeed.number = 1;
            Inventory_System.Instance.AddSeedToInv(NewSeed);
        } 
        else 
        {
            Inventory_System.Instance.Planttest[seed_inventory_index].number++;
        }
    }
    private int FindSeedIndexInInv(PlantData Plant_data)
    {
        for (int i = 0; i < Inventory_System.Instance.Planttest.Count; ++i)
        {
            if (Plant_data.plantName == Inventory_System.Instance.Planttest[i].plant.plantName)
            {
                return i;
            }
        }
        // Ni ma OK
        return -1;
    }
    private void UpdateStoreUI()
    {
        foreach(Transform child in StoreUI.transform) {
            GameObject.Destroy(child.gameObject);
        }

        foreach(seed s in magazyn)
        {
            Button b = Instantiate(buttonPref, StoreUI.transform);
            b.GetComponent<Image>().sprite = s.plant.seedIcon;
            b.GetComponent<SeedInfo>().Seed = s.plant;
            b.onClick.AddListener(delegate { Buy(s); });

        }

    }
}
