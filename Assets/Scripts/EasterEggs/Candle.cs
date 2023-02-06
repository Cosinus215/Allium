using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Candle : MonoBehaviour, IInteractable
{
    [SerializeField] private Light fire;
    [SerializeField] private UnityEvent toRise;
    public bool IsLit = true;

    void Start()
    {
        fire.enabled = IsLit;
    }
    public bool Interact(Items item = null)
    {
        switch (item.itemType)
        {
            case Items.Type.Axe:
                Debug.Log("Ciupaga");
                return false;

            case Items.Type.Hoe:
                return false;

            case Items.Type.Shovel:
                Debug.Log("Lopata");
                return false;

            case Items.Type.Can:
                fire.enabled = !fire.enabled;
                IsLit = fire.enabled;
                toRise.Invoke();
                return true;

            case Items.Type.Bundle:
                    return false;

            default:
                Debug.Log("Shitty item");
                return false;
        }
    }
    public void LitFire()
    {
        fire.enabled = true;
        IsLit = true;
    }
}
