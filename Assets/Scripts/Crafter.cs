using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class Recipe
{
    public string name;

    public GameObject producedItem;

    public List<ItemSO> requiredItems;
}

public class Crafter : MonoBehaviour
{
    public float craftTime = 2f;
    public List<Recipe> recipes;

    private bool isCrafting;

    private Dictionary<ItemSO, int> inventory = new Dictionary<ItemSO, int>();

    private List<Transform> startConveyors = new List<Transform>();
    private int outputIndex;

    private HashSet<Item> ignoredItems = new HashSet<Item>();
    public float ingoredDuration = 2f;

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

        if (collision.CompareTag("Item"))
        {
            Item item = collision.GetComponent<Item>();
            if (item == null || item.itemType == null)
                return;

            if (ignoredItems.Contains(item)) return;

            item.beingTransfered = true;

            if (!inventory.ContainsKey(item.itemType))
                inventory[item.itemType] = 0;

            inventory[item.itemType]++;

            Destroy(item.gameObject);

            TryCraft();
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

    void TryCraft()
    {
        if (isCrafting) return;

        Recipe recipe = FindCraftableRecipe();
        if (recipe == null) return;

        StartCoroutine(Craft(recipe));
    }

    Recipe FindCraftableRecipe()
    {
        foreach(Recipe recipe in recipes)
        {
            if (recipe.producedItem == null) continue;

            if (HasIngrediant(recipe))
                return recipe;
        }

        return null;
    }

    bool HasIngredients(Recipe recipe)
    {
        foreach(ItemSO required in recipe.requiredItems)
        {
            if (!inventory.TryGetValue(required), out int count) || count <= 0)
                return false;
        }
        
        return true
    }

    void ConsumeIngredients(Recipe recipe)
    {
        foreach (ItemSO requied in recipe.requiredItems)
        {
            inventory[requied]--;
        }
    }

    IEnumerator Craft(Recipe recipe)
    {
        isCrafting = true;

        ConsumeIngredients(recipe);

        yield return new WaitForSeconds(craftTime);

        if (startConveyors.Count > 0)
        {
            Transform output = startConveyors[outputIndex];
            outputIndex = (outputIndex + 1) % startConveyors.Count;
            GameObject obj = Instantiate(recipe.producedItem, output.position, Quaternion.identity);

            Item newItem = obj.GetComponent<Item>();
            if (newItem != null)
            {
                ignoredItems.Add(newItem);
                StartCoroutine(RemoveIgnoredItemAfterDelay(newItem));
            }
        }

        isCrafting = false;

        TryCraft();
    }
    
    IEnumerator RemoveIgnoredItemAfterDelay(Item item)
    {
        yield return new WaitForSeconds(IgnoreDuration);

        ignoredItems.Remove(item);
    }
}
