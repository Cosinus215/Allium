using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Inventory_System : MonoBehaviour {
    public static Inventory_System Instance;

    public List<seed> Bundle_Inv = new List<seed>();
    [SerializeField] private List<Items> Eq = new List<Items>();
    [SerializeField] private Items currentItem;
    [SerializeField] private GameObject UI_Eq;
    [SerializeField] private GameObject selectingSquare;
    [SerializeField] private GameObject Bundle_Selecting_Square;
    [SerializeField] private GameObject handPlace;
    [SerializeField] private Transform point;
    [SerializeField] private Vector3 hitboxSize;
    [SerializeField] private Coins Coin;
    [SerializeField] private TextMeshProUGUI MoneyUI;
    [SerializeField] private Image[] invSprites;
    [HideInInspector] public int SeedNumber;
    [SerializeField] private ParticleSystem[] effects;
    public GameObject Bundle;
    public float nextClick = 0;
    private void Awake()
    {
        if (Instance is null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Too much inventories");
            Destroy(this);
            return;
        }
    }
    private void Start() 
    {
        if (Eq.Count > 0) 
        {
            handPlace.GetComponent<MeshRenderer>().enabled = true;

            for (int i=0; i<invSprites.Length; i++)
            {
                if (i < Eq.Count)
                {
                    invSprites[i].sprite = Eq[i].Icon;
                }
                else
                {
                    invSprites[i].sprite = null;
                }
                
            }
            if (Eq[0]) {
                handPlace.GetComponent<MeshRenderer>().material.SetTexture("_BaseMap", Eq[0].Texture);
            }
        }

        UpdateMoneyUI();
        ChangeHoldedItem(1);
        UpdateBundleUI();
    }

    /*
    Wylaczone do czasu refaktora
    private void OnTriggerEnter(Collider collider) {
        if (collider.TryGetComponent<Item_info>(out Item_info item_info)) {
            Pickup_Item(item_info, collider);
        }Wylaczone do czasu refaktora
    }
    
    void Pickup_Item(Item_info item_info, Collider collider) {
        Items newItem = item_info.itemToPickup;

        for (int i = 0; i < UI_Eq.transform.childCount; i++) {
            Image Slot = UI_Eq.transform.GetChild(i).gameObject.GetComponent<Image>();
            if (Slot.sprite == null) {

                Eq.Add(newItem);
                Slot.sprite = newItem.Icon;

                Destroy(collider.gameObject);
                break;
            }
        }
    }
    */
    public void Change_slot(InputAction.CallbackContext context) {
        if (context.started) {
            int selection_number = Mathf.Clamp(int.Parse(context.control.name),0, invSprites.Length);

            

            ChangeHoldedItem(selection_number);
        }
    }
    
    public void ChangeHoldedItem(int itemID)
    {
        Mathf.Clamp(itemID, 1, invSprites.Length);//mozemy wybierac tylko z widocznych slotow

        selectingSquare.transform.SetParent(invSprites[itemID - 1].transform);
        selectingSquare.transform.localPosition = Vector2.zero;

        if (itemID <= Eq.Count)
        {
            handPlace.GetComponent<MeshRenderer>().material.SetTexture("_BaseMap", Eq[itemID - 1].Texture);
            currentItem = Eq[itemID - 1];
        }

        handPlace.GetComponent<MeshRenderer>().enabled = (itemID <= Eq.Count);

        if (Eq[itemID - 1].itemType == Items.Type.Bundle)
        {
            UpdateBundleUI();
            Bundle.SetActive(true);
        }
        else
        {
            Bundle.SetActive(false);
        }
    }
    
    public void Action(InputAction.CallbackContext context)
    {
        if (context.started && point && Time.time>= nextClick)
        {       
            Collider[] hit = Physics.OverlapBox(point.position, hitboxSize);
            for (int i = 0;i < hit.Length; ++i) 
            {  
                if (hit[i].gameObject.TryGetComponent(out IInteractable inter))
                {
                    bool b = inter.Interact(currentItem);
                    if (b) {
                        UpdateBundleUI();
                    }
                    break;
                }
            }
            int a = currentItem.EffectNumber;
            if (a != -1 && a < effects.Length)
            {
                effects[a].Play();
            }
            nextClick = Time.time + 1;
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
        Bundle_Selecting_Square.transform.SetParent(Bundle.transform.GetChild(i));
        Bundle_Selecting_Square.transform.localPosition = Vector2.zero;
    }

    public seed GetSeed() {
        if (Bundle_Inv.Count > 0 && SeedNumber< Bundle_Inv.Count)
        {
            return Bundle_Inv[SeedNumber];
        }
        return null;
    }

    public void UpdateBundleUI() {
        for (int i = 0; i < 9; i++) 
        {
            Transform slot = Bundle.transform.GetChild(i);
            //GetComponentInChildren nie dzia³a lol
            Image iconDisplay = slot.GetChild(0).GetComponent<Image>();
            TextMeshProUGUI number = slot.GetComponentInChildren<TextMeshProUGUI>();

            if (i < Bundle_Inv.Count && Bundle_Inv[i]!=null) 
            {
                iconDisplay.sprite = Bundle_Inv[i].plant.GetPlantIcon();
                iconDisplay.color = new Color(1, 1, 1, 1);
                number.SetText(Bundle_Inv[i].number.ToString());
                if (Bundle_Inv[i].number == 0) 
                {
                    number.SetText("");
                }
            } 
            else 
            {
                iconDisplay.sprite = null;
                iconDisplay.color = new Color(0,0,0,0);
                number.SetText("");
            }
        }
    }

    public void Buy(int id) { 
        PlantData Seedinfo = Store.Instance.magazyn[id].plant;

        //czy mamy kase
        if (Coin.Money >= Seedinfo.Price) {
            int seed_inventory_index = FindSeedIndexInInv(Seedinfo);

            if (seed_inventory_index == -1) {
                seed NewSeed = new seed();
                NewSeed.plant = Seedinfo;
                NewSeed.number = 1;
                AddSeedToInv(NewSeed);
            } else {

                Bundle_Inv[seed_inventory_index].number++;
            }
            Coin.Money -= Seedinfo.Price;
            UpdateMoneyUI();
            UpdateBundleUI();

        } else {
            Debug.Log("You have too little money");
        }
    }

    private int FindSeedIndexInInv(PlantData Plant_data) {
        for (int i = 0; i < Bundle_Inv.Count; ++i) {
            if (Plant_data.plantName == Bundle_Inv[i].plant.plantName) {
                return i;
            }
        }
        // Ni ma OK
        return -1;
    }

    public void AddSeedToInv(seed newSeed)
    {
        if(newSeed!=null && newSeed.plant != null)
        {
            Bundle_Inv.Add(newSeed);
            UpdateBundleUI();
        }
        else
        {
            Debug.LogError("Broken seed!");
        }  
    }
    public void AddMoney(int amout)
    {
        Coin.Money += amout;
        UpdateMoneyUI();
    }
    public void SetMoney(int amout)
    {
        Coin.Money = amout;
        UpdateMoneyUI();
    }
    public void UpdateMoneyUI() {
        MoneyUI.SetText($"Coins: {Coin.Money}");
    }
    public int GetMoneyAmout()
    {
        return Coin.Money;
    }
    public void ClearSeedEq()
    {
        Bundle_Inv.Clear();
    }
}

[System.Serializable]
public class seed {
    public int number;
    public PlantData plant;

    public seed()
    {
    }
    public seed(SeedSaveData ssd)
    {
        this.number = ssd.amount;
        this.plant = GameManager.Instance.GetPlantDataByID(ssd.plantID);
    }

}
