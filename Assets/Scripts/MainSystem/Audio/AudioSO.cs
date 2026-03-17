// fsharp_align_function_signature_to_indentation = false

using System;
using System.Collections.Generic;
using UnityEngine;

namespace MainSystem.Audio
{
[CreateAssetMenu(menuName = "ScriptableObject/AudioSO")]
public class AudioSO : ScriptableObject
{
    [SerializeField] List<SoundData> _seSounds;
    [SerializeField] List<SoundData> _bgmSounds;
    [SerializeField] List<SoundData> _jingleSounds;
    

    public List<SoundData> SESounds => _seSounds;
    public List<SoundData> BGMSounds => _bgmSounds;
    public List<SoundData> JingleSounds => _jingleSounds;
    
}
}