using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Store : MonoBehaviour {
    [SerializeField] private GameObject StoreUI;

    private void OnTriggerEnter(Collider collider) {
        StoreUI.SetActive(true);
    }

    private void OnTriggerExit(Collider collider) {
        StoreUI.SetActive(false);
    }

    public void Buy() {
        PlantData Seedinfo = EventSystem.current.currentSelectedGameObject.GetComponent<SeedInfo>().Seed;

        int seed_inventory_index = FindSeedIndexInInv(Seedinfo);

        if (seed_inventory_index == -1) {
            seed NewSeed = new seed();
            NewSeed.plant = Seedinfo;
            NewSeed.number = 1;
            Inventory_System.Instance.Planttest.Add(NewSeed);
            Inventory_System.Instance.UpgradeBundleUI();

        } else {
            Inventory_System.Instance.Planttest[seed_inventory_index].number++;
        }
    }

    private int FindSeedIndexInInv(PlantData Plant_data) {
        for (int i = 0; i < Inventory_System.Instance.Planttest.Count; ++i) {
            if (Plant_data.plantName == Inventory_System.Instance.Planttest[i].plant.plantName) {
                return i;
            }
        }
        // C# nullable method
        return -1;
    }
}
