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
        SeedInfo thisBtn = EventSystem.current.currentSelectedGameObject.GetComponent<SeedInfo>();

        PlantData Seed = ScriptableObject.CreateInstance<PlantData>();

        Seed.plantName = thisBtn.SeedName;
        Seed.plantIcon = thisBtn.SeedIcon;
        Seed.plantTextures = thisBtn.plantTextures;
        Seed.plantGrowthTime = thisBtn.plantGrowthTime;

        for (int i = 0; i < Inventory_System.Instance.Planttest.Count; i++) {
            if (Seed.plantName == Inventory_System.Instance.Planttest[i].plant.plantName) {
                Inventory_System.Instance.Planttest[i].number++;
                return;
            } 
            if (i+1 == Inventory_System.Instance.Planttest.Count) {
                seed NewSeed = new seed();
                NewSeed.plant = Seed;
                NewSeed.number = 1;
                Inventory_System.Instance.Planttest.Add(NewSeed);

                Inventory_System.Instance.UpgradeBundleUI();
                return;
            }
        }
    }
}
