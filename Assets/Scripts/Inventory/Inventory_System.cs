using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Inventory_System : MonoBehaviour {
    public static Inventory_System Instance;

    public List<seed> Planttest = new List<seed>();
    [SerializeField] private List<Items> Eq = new List<Items>();
    [SerializeField] private Items currentItem;
    [SerializeField] private GameObject UI_Eq;
    [SerializeField] private GameObject selectingSquare;
    [SerializeField] private GameObject handPlace;
    [SerializeField] private Transform point;
    [SerializeField] private Vector3 hitboxSize;
    public GameObject Bundle;
    public int SeedNumber;

    private void Start() {
        UpgradeBundleUI();

        if (Instance is null) {
            Instance = this;
        } else {
            Debug.LogWarning("Too much inventories");
            Destroy(this);
            return;
        }
        
        if (Eq.Count > 0) {
            handPlace.GetComponent<MeshRenderer>().enabled = true;

            for (int i = 0; i < Eq.Count; i++) {
                GameObject Slot = UI_Eq.transform.GetChild(i).gameObject;
                if (Slot.GetComponent<Image>().sprite == null) {
                    Slot.GetComponent<Image>().sprite = Eq[i].Icon;
                }
            }

            if (Eq[0]) {
                handPlace.GetComponent<MeshRenderer>().material.SetTexture("_BaseMap", Eq[0].Texture);
            }
        }
        
        ChangeHoldedItem(1);
    }


    private void OnTriggerEnter(Collider collider) {
        if (collider.TryGetComponent<Item_info>(out Item_info item_info)) {
            Pickup_Item(item_info, collider);
        }
    }

    void Pickup_Item(Item_info item_info, Collider collider) {
        Items newItem = item_info.itemToPickup;

        for (int i = 0; i < UI_Eq.transform.childCount; i++) {
            GameObject Slot = UI_Eq.transform.GetChild(i).gameObject;
            if (Slot.GetComponent<Image>().sprite == null) {

                Eq.Add(newItem);
                Slot.GetComponent<Image>().sprite = newItem.Icon;

                Destroy(collider.gameObject);
                break;
            }
        }
    }

    public void Change_slot(InputAction.CallbackContext context) {
        if (context.started) {
            int selection_number = int.Parse(context.control.name);

            if (Eq[selection_number-1].itemType == Items.Type.Bundle) {
                Bundle.SetActive(true);
            } else {
                Bundle.SetActive(false);
            }

            ChangeHoldedItem(selection_number);
        }
    }
    
    private void ChangeHoldedItem(int itemID)
    {
        selectingSquare.transform.parent = UI_Eq.transform.GetChild(itemID - 1);
        selectingSquare.transform.localPosition = Vector2.zero;

        if (itemID <= Eq.Count)
        {
            handPlace.GetComponent<MeshRenderer>().enabled = true;
            handPlace.GetComponent<MeshRenderer>().material.SetTexture("_BaseMap", Eq[itemID - 1].Texture);
            currentItem = Eq[itemID - 1];
        }
        else
        {

            handPlace.GetComponent<MeshRenderer>().enabled = false;
        }
    }
    
    public void Action(InputAction.CallbackContext context)
    {
        if (context.started && point)
        {
            Collider[] hit = Physics.OverlapBox(point.position, hitboxSize);
            Debug.Log($"Used: {currentItem.Name} {hit[0]}");
            for (int i = 0;i < hit.Length; ++i) 
            {
                
                if (hit[i].gameObject.TryGetComponent(out IInteractable inter))
                {
                    bool b = inter.Interact(currentItem);
                    return;
                }
            }
        }
    }
    void OnDrawGizmos()
    {
        if (point)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(point.position, hitboxSize);
        }
    }

    public void GetSeedNumber(int i) {
        SeedNumber = i;
    }

    public seed GetSeed() {
        return Planttest[SeedNumber];
    }


    public void UpgradeBundleUI() {
        for (int i = 0; i < Planttest.Count; i++) {
            Bundle.transform.GetChild(i).GetComponent<Image>().sprite = Planttest[i].plant.GetPlantIcon();
        }
    }
}

[System.Serializable]
public class seed {
    public int number;
    public PlantData plant;
}
