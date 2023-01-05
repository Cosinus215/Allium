using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Store : MonoBehaviour {
    public static Store Instance;

    [SerializeField] private GameObject StoreUI;
    [SerializeField] private Button buttonPref;
    [HideInInspector] public List<seed> magazyn = new List<seed>();

    private void Start() {
        if (Instance is null) {
            Instance = this;
        } else {
            Debug.LogWarning("Too much Stores");
            Destroy(this);
            return;
        }
    }

    private void OnTriggerEnter(Collider collider) {
        StoreUI.SetActive(true);
        UpdateStoreUI();
    }

    private void OnTriggerExit(Collider collider) {
        StoreUI.SetActive(false);
    }

    private void UpdateStoreUI()
    {
        foreach(Transform child in StoreUI.transform) {
            GameObject.Destroy(child.gameObject);
        }

        int number = 0;
        foreach(seed s in magazyn)
        {
            int temp = number;
            Button b = Instantiate(buttonPref, StoreUI.transform);
            b.GetComponent<Image>().sprite = s.plant.seedIcon;
            b.GetComponent<SeedInfo>().Seed = s.plant;
            b.onClick.AddListener(delegate { Inventory_System.Instance.Buy(temp); });
            number++;
        }

    }
}
