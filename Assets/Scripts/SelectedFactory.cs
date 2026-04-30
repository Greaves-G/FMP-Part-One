using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class SelectedFactory : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler 
{
    public static GameObject factorySelected;
    public static List<Slot> factorySelectedRequiredItems = new List<Slot>();

    public List<Slot> requiredItems = new List<Slot>();

    public static bool isDeleting;

    public AudioClip errorSFX;
    public void SetSelectedFactory(GameObject factoryToSelect)
    {
        if (!Storage.Instance.HasItem(requiredItems))
        {
            //play error SFX
            return;
        }

        factorySelected = factoryToSelect;
        factorySelectedRequiredItems = requiredItems;
        isDeleting = false;
    }

    public void EnableIsDeleting()
    {
        isDeleting = true;
        factorySelected = null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        BuildingCostHolder.Instance.parentObj.SetActive(true);
        BuildingCostHolder.Instance.UpdateCost(requiredItems);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        BuildingCostHolder.Instance.parentObj.SetActive(false);
    }
}
