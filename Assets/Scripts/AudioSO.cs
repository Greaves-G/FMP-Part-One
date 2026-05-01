using UnityEngine;

public enum audioClipType { None, Build, Hover, Click, Error }
[CreateAssetMenu(fileName = "Audio", menuName = "NewAudio")]
public class AudioSO : ScriptableObject
{
    public AudioClip clip;
    public audioClipType clipType;
    public int maxPlayerAtOnce;
    public float startPlayingSpacing;
}
