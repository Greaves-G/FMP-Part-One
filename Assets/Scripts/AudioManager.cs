using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Rendering.Universal;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public List<AudioSO> AudioSOs = new List<AudioSO>();
    public Dictionary<AudioSO, int> activeAudioClips = new Dictionary<AudioSO, int>();
    public Dictionary<AudioSO, float> _lastPlayedTimes = new Dictionary<AudioSO, float>();

    private void Awake()
    {
        instance = this;

        foreach (AudioSO audioSO in AudioSOs)
        {
            activeAudioClips.Add(audioSO, 0);
            _lastPlayedTimes.Add(audioSO, 0);
        }
    }

    public void PlaySFX(audioClipType clipType, float volume = 1f)
    {
        AudioSO clipSO = clipTypeToAudioSO(clipType);

        if (clipSO == null) return;

        if (TryPlay(clipSO))
            StartCoroutine(PlayAndDestory(clipSO, volume));
    }

    AudioSO clipTypeToAudioSO(audioClipType clipType)
    {
        foreach (AudioSO audioSO in AudioSOs)
        {
            if (audioSO.clipType == clipType)
                return audioSO;
        }

        return null;
    }

    bool TryPlay(AudioSO clipSO)
    {
        int audioIndex = AudioSOs.IndexOf(clipSO);
        AudioSO audioSO = AudioSOs[audioIndex];

        if (activeAudioClips[clipSO] >= audioSO.maxPlayerAtOnce && audioSO.maxPlayerAtOnce != 0)
            return false;

        var lastPlayed = _lastPlayedTimes[clipSO];

        if (Time.time < lastPlayed + audioSO.startPlayingSpacing)
            return false;

        _lastPlayedTimes[clipSO] = Time.time;

        return true;
    }

    IEnumerator PlayAndDestory(AudioSO clipSO, float volume)
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();

        AudioClip clip = clipSO.clip;
        activeAudioClips[clipSO] += 1;

        source.clip = clip;
        source.volume = volume;
        source.playOnAwake = false;
        source.spatialBlend = 0f;

        source.Play();

        yield return new WaitForSeconds(clip.length * 2);

        activeAudioClips[clipSO] -= 1;

        Destroy(source);
    }
}
