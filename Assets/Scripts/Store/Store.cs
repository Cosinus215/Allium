using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Store : MonoBehaviour {
    public static Store Instance;

    [SerializeField] private GameObject StoreUI;
    [SerializeField] private GameObject store_Place;
    [SerializeField] private GameObject buttonPref;
    [SerializeField] public List<seed> magazyn = new List<seed>();

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
        foreach(Transform child in store_Place.transform) {
            GameObject.Destroy(child.gameObject);
        }

        int number = 0;
        foreach(seed s in magazyn)
        {
            int temp = number;
            if (!buttonPref) {
                Debug.LogWarning("Nie ma prefaba");
                return; 
            }
            GameObject gameobj = Instantiate(buttonPref, store_Place.transform);
            Button b = gameobj.transform.GetChild(0).GetComponent<Button>();
            if (!b) {
                Debug.LogWarning("Nie ma buttona");
                return; 
            }

            b.GetComponent<Image>().sprite = s.plant.seedIcon;
            b.GetComponent<SeedInfo>().Seed = s.plant;
            b.transform.Find("Price").GetComponent<TextMeshProUGUI>().SetText(s.plant.Price.ToString());
            b.transform.Find("Desc").GetComponent<TextMeshProUGUI>().SetText(s.plant.GetPlantName());
            b.onClick.AddListener(delegate { Inventory_System.Instance.Buy(temp); });
            number++;
        }

    }
}
