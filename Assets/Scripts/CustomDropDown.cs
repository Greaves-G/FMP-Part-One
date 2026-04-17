using UnityEngine;
using UnityEngine.EventSystems;

public class CustomDropDown : MonoBehaviour, IPointerClickHandler
{
    public GameObject parent;

    public void OnPointerClick(PointerEventData eventData)
    {
        parent.SetActive(!parent.activeInHierarchy);
    }
}
