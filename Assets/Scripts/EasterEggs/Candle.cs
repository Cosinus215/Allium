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
                return false;

            case Items.Type.Hoe:
                return false;

            case Items.Type.Shovel:
                return false;

            case Items.Type.Can:
                fire.enabled = false;
                IsLit = false;
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
