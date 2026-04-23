using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System;
public class Item : MonoBehaviour
{
    public bool beingTransfered;

    public ItemSO itemType;

    private Dictionary<Conveyor, float> conveyorTimeStamps = new Dictionary<Conveyor, float>();

    public bool HasVisistedRecently(Conveyor converyor, float cooldown)
    {
        if (conveyorTimeStamps.TryGetValue(converyor, out float lastTime))
        {
            if (Time.time - lastTime > cooldown) return true;
        }
        return false;
    }

    public void MarksVisited(Conveyor converyor)
    {
        conveyorTimeStamps[converyor] = Time.time;
    }
}