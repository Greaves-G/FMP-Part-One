using UnityEngine;
using UnityEngine.EventSystems;

public class UIAudioSFX : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    /*public AudioClip hoverClip;
    public AudioClip clickClip;*/

    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.instance.PlaySFX(audioClipType.Hover);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.instance.PlaySFX(audioClipType.Click);
    }
}
