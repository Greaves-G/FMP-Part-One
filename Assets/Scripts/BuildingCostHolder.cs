using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using Unity.VisualScripting;

public class BuildingCostHolder : MonoBehaviour
{
    public static BuildingCostHolder Instance;

    public Vector2 offSet;
    public GameObject parentObj;

    public List<Image> icons = new List<Image>();
    public List<TextMeshProUGUI> amount = new List<TextMeshProUGUI>();

    private RectTransform rectTransform;
    private void Awake()
    {
        Instance = this;
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        rectTransform.position = (Vector2)Input.mousePosition + offSet;
    }

    public void UpdateCost(List<Slot> requiredItems)
    {
        for (int i = 0; i < icons.Count; i++)
        {
            bool active = requiredItems.Count > i;

            icons[i].enabled = active;
            if (active)
                icons[i].sprite = requiredItems[i].type.icon;
            else
                icons[i].sprite = null;

            amount[i].text = active ? requiredItems[i].amount.ToString() : "";
        }
    }
}
