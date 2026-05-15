using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Miner : MonoBehaviour
{
    //public static float extractTime = 2f;

    private GridPosition gridPosition;
    private readonly List<Conveyor> conveyors = new List<Conveyor>();
    private int conveyorIndex = 0;

    private GameObject cachedItemPrefab;

    private void Awake()
    {
        gridPosition = GetComponent<GridPosition>();
    }

    private void Start()
    {
        cacheBiome();
    }

    private void OnEnable()
    {
        StartCoroutine(Running());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    void cacheBiome()
    {
        if (WorldGen.instance == null)
        {
            Debug.LogError("WorldGen.instance is NULL");
            return;
        }

        if (gridPosition == null)
        {
            Debug.LogError("GridPosition is NULL on Miner");
            return;
        }

        int halfSized = WorldGen.instance.mapSize / 2;

        int x = gridPosition.position.x + halfSized;
        int y = gridPosition.position.y + halfSized;

        if (WorldGen.instance.biomeMap == null)
        {
            Debug.LogError("biomeMap is NULL");
            return;
        }

        if (x < 0 || y < 0 || x >= WorldGen.instance.mapSize || y >= WorldGen.instance.mapSize)
            return;

        Biome biome = WorldGen.instance.biomeMap[x, y];

       

        cachedItemPrefab = biome.itemPrefab;
    }


    IEnumerator Running()
    {
        while (true)
        {
            yield return new WaitForSeconds(UpgradeManager.Instance.minerExtractTime);
            extract();
        }
    }


    void extract()
    {
        if (conveyors.Count == 0) return;

        Conveyor target = conveyors[conveyorIndex];
        conveyorIndex = (conveyorIndex + 1) % conveyors.Count; 

        Instantiate(cachedItemPrefab, target.transform.position, Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Conveyor conveyor = collision.GetComponent<Conveyor>();
        if (conveyor != null && !conveyors.Contains(conveyor))
        {
            conveyors.Add(conveyor);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Conveyor conveyor = collision.GetComponent<Conveyor>();
        if (conveyor != null)
        {
            conveyors.Remove(conveyor);
            conveyorIndex %= Math.Max(conveyors.Count, 1);
        }
    }
}
