using System;
using UnityEngine;

namespace MainSystem.Audio
{
[CreateAssetMenu(menuName = "ScriptableObject/SoundData")]
public class SoundData : ScriptableObject
{
    [SerializeField] AudioCategory category;
    [SerializeField] string soundName;
    [SerializeField] AudioClip clip;
    [SerializeField,Range(0f, 5f)] float volume = 1f;
    [SerializeField] bool loop = false;
    
    public AudioCategory Category => category;
    public string SoundName => soundName;
    public AudioClip Clip => clip;
    public float Volume => volume;
    public bool Loop => loop;

    void Reset()
    {
        soundName = name;
    }
}
}
