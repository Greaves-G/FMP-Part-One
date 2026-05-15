using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



[System.Serializable]
public class Slot
{
    public ItemSO type;
    public int amount;

    public Slot(ItemSO type, int amount)
    {
        this.type = type;
        this.amount = amount;
    }
}
public class Storage : MonoBehaviour
{
    public static Storage Instance;
    public List<Slot> slots = new List<Slot>();

    private List<GameObject> instantiatedItems = new List<GameObject>();

    public GameObject itemPrefab;
    public Transform grid;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        UpdateDisplay();
    }


    public void AddItem(ItemSO itemType, int amount)
    {
        if (amount < 0) return;

        foreach (Slot slot in slots)
        {
            if (slot.type == itemType)
            {
                slot.amount += amount;
                    return;
            }
        }

        slots.Add(new Slot(itemType, amount));
    }

    public bool HasItem(List<Slot> requiredItems)
    {

        foreach (Slot required in requiredItems)
        {
            Slot found = slots.Find(s => s.type == required.type);

            if (found == null || found.amount < required.amount) return false;
        }
        
        return true;
    }

    public void RemoveItems(List<Slot> requiredItems)
    {
        foreach (Slot required in requiredItems)
        {
            Slot found = slots.Find(s => s.type == required.type);

            if (found != null)
            {
                found.amount -= required.amount;

                if (found.amount <= 0)
                {
                    slots.Remove(found);
                }
            }
        }
    }

    public void UpdateDisplay()
    {
        foreach (GameObject go in instantiatedItems)
        {
            Destroy(go);
        }

        instantiatedItems.Clear();

        foreach (Slot slot in slots)
        {
            GameObject itemGO = Instantiate(itemPrefab, grid);
            instantiatedItems.Add(itemGO);

            Image icon = itemGO.GetComponent<Image>();
            if (icon != null)
                icon.sprite = slot.type.icon;

            TextMeshProUGUI amountText = itemGO.GetComponentInChildren<TextMeshProUGUI>();
            if (amountText != null)
                amountText.text = slot.amount.ToString();
        }
    }
}
