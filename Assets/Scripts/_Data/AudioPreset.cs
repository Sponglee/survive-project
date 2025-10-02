using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioPreset", menuName = "Scriptable Objects/AudioPreset")]
public class AudioPreset : ScriptableObject
{
    public List<AudioPair> AudioClips;
}

[Serializable]
public class AudioPair
{
    public string Key;
    public AudioClip AudioClip;
}
