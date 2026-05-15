using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class Smelter : MonoBehaviour
{
    //public float SmeltTime = 2.0f;

    private bool isProcessing;

    private readonly List<Transform> startConveyors = new List<Transform>();
    private int outputIndex = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("StartConveyor"))
        {
            Transform t = collision.transform;
            if (!startConveyors.Contains(t))
            {
                startConveyors.Add(t);
            }
            return;
        }
        
        if(!isProcessing && collision.CompareTag("Item"))
        {
            Item item = collision.GetComponent<Item>();
            if (item == null || item.itemType == null)
                return;

            if (item.itemType.smeltedItem == null)
                return;

            StartCoroutine(Smelt(item));
                       
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("StartConveyor"))
        {
            startConveyors.Remove(collision.transform);
            outputIndex %= Mathf.Max(startConveyors.Count, 1);
        }
    }

    IEnumerator Smelt(Item item)
    {
        isProcessing = true;

        float timer = 0f;

        while (timer < UpgradeManager.Instance.FurnaceSmeltSpeed)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        //Debug.Log("Timer: " + timer + " / " + UpgradeManager.Instance.FurnaceSmeltSpeed);

        //yield return new WaitForSeconds(UpgradeManager.Instance.FurnaceSmeltSpeed);

        if (item != null) 
            Destroy(item.gameObject);

        if(startConveyors.Count == 0)
        {
            isProcessing = false;
            yield break;
        }

        Transform output = startConveyors[outputIndex];
        outputIndex = (outputIndex + 1) % startConveyors.Count;

        Instantiate(item.itemType.smeltedItem, output.position, Quaternion.identity);

        isProcessing = false;
    }
}
