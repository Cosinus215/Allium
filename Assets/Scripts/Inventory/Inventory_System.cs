using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Inventory_System : MonoBehaviour {
    [SerializeField] private List<Items> Eq = new List<Items>();
    [SerializeField] private GameObject UI_Eq;
    [SerializeField] private GameObject selectingSquare;

    private void OnTriggerEnter(Collider collider) {
        if (collider.TryGetComponent<Item_info>(out Item_info item_info)) {
            Items newItem = ScriptableObject.CreateInstance<Items>();
            newItem.Name = item_info.Name;
            newItem.Icon = item_info.Icon;

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
    }

    public void Change_slot(InputAction.CallbackContext context) {
        if (context.started) {
            selectingSquare.transform.parent = UI_Eq.transform.GetChild(int.Parse(context.control.name) - 1);
            selectingSquare.transform.localPosition = Vector2.zero;
        }
    }
}
