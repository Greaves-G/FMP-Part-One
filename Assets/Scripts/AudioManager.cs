using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal.Internal;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;

        StartCoroutine(PlayAndDestory(clip, volume));
    }

    IEnumerator PlayAndDestory(AudioClip clip, float volume)
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();

        source.clip = clip;
        source.volume = volume;
        source.playOnAwake = false;
        source.spatialBlend = 0f;

        source.Play();

        yield return new WaitForSeconds(clip.length * 2);

        Destroy(source);
    }
}
