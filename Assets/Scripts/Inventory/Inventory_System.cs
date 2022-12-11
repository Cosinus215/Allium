using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Inventory_System : MonoBehaviour {
    [SerializeField] private List<Items> Eq = new List<Items>();
    [SerializeField] private GameObject UI_Eq;
    [SerializeField] private GameObject selectingSquare;
    [SerializeField] private GameObject handPlace;

    private void Start() {
        if (Eq.Count > 0) {
            handPlace.GetComponent<MeshRenderer>().enabled = true;

            for (int i = 0; i < UI_Eq.transform.childCount; i++) {
                GameObject Slot = UI_Eq.transform.GetChild(i).gameObject;
                if (Slot.GetComponent<Image>().sprite == null) {
                    Slot.GetComponent<Image>().sprite = Eq[i].Icon;
                    break;
                }
            }

            if (Eq[0]) {
                handPlace.GetComponent<MeshRenderer>().material.SetTexture("_BaseMap", Eq[0].Texture);
            }
        }
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

            selectingSquare.transform.parent = UI_Eq.transform.GetChild(selection_number - 1);
            selectingSquare.transform.localPosition = Vector2.zero;

            if (selection_number <= Eq.Count) {
                handPlace.GetComponent<MeshRenderer>().enabled = true;
                //handPlace.GetComponent<MeshRenderer>().material = Eq[selection_number - 1].Texture;
                handPlace.GetComponent<MeshRenderer>().material.SetTexture("_BaseMap", Eq[selection_number - 1].Texture);
            } else {

                handPlace.GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }
}
