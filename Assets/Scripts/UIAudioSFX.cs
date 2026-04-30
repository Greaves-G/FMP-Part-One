using UnityEngine;
using UnityEngine.EventSystems;

public class UIAudioSFX : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public AudioClip hoverClip;
    public AudioClip clickClip;

    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.instance.PlaySFX(hoverClip);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.instance.PlaySFX(clickClip);
    }
}
