using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;


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

    private void Awake()
    {
        Instance = this;
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
}
