using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;

namespace MainSystem.Audio
{
/// <summary> オーディオの大まかなカテゴリ </summary>
public enum AudioCategory
{
    Master,
    BGM,
    SE,
    /// <summary> SEより長いサウンド </summary>
    Jingle
}

public interface IAudioManager
{
    UniTask PlayBGM(string bgmKey, float fadeTime = 0f,float volume = 1f);
    void StopBGM(float fadeTime = 0f);
    void PlaySE(string seKey, float volume = 1f);
    void PlayJingle(string jingleKey, float volume = 1f);
    
    void PlayLoopSE(string seKey, float volume = 1f);
    void StopLoopSE(string seKey);
   
    void SetVolume(AudioCategory category, float volume);
    float GetVolume(AudioCategory category);
}

//3D以外のオーディオ管理を行うマネージャークラス
//SE、BGM、ボイスなど
public class AudioManager : MonoBehaviour, IAudioManager
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] AudioSO audioSO;
    
    AudioSource _bgmSource;
    List<AudioSource> _seSources = new();
    
    const int StartSESourceCount = 4;

    CancellationTokenSource _bgmFadCTS;
    
    void Awake()
    {
        _bgmSource = CreateAudioSource("BGM", true);
        
        for (int i = 0; i < StartSESourceCount; i++)
        {
            _seSources.Add(CreateAudioSource("SE", false));
        }
    }
    
    AudioSource CreateAudioSource(string mixerGroup, bool loop)
    {
        var source = gameObject.AddComponent<AudioSource>();
        source.outputAudioMixerGroup = audioMixer.FindMatchingGroups(mixerGroup)[0];
        source.loop = loop;
        source.spatialBlend = 0f; // 2D
        return source;
    }
    
    public async UniTask PlayBGM(string bgmKey, float fadeTime = 0f, float volume = 1f)
    {
        AudioClip clip = audioSO.BGMSounds.Find(s => s.SoundName == bgmKey)?.Clip;
        if (clip == null) return;
        

        _bgmFadCTS?.Cancel();
        if (fadeTime != 0)
        {
            fadeTime /= 2f;//フェードアウトとフェードインで分割
        }
        
        if (_bgmSource.isPlaying)
        {
            //BGMが再生中ならフェードアウト
            if (fadeTime > 0f)
            {
                _bgmFadCTS = new CancellationTokenSource();
                await FadeOutBGM(fadeTime, _bgmFadCTS);
            }
            else
            {
                _bgmSource.volume = volume;
                _bgmSource.Stop();
            }
        }
        //新しいBGMをセットしてフェードイン
        if (fadeTime > 0f)
        {
            _bgmSource.volume = 0f;
            _bgmSource.clip = clip;
            _bgmSource.Play();
            _bgmSource.DOFade(volume, fadeTime);
            await  UniTask.Delay((int)(fadeTime * 1000));
        }
        else
        {
            _bgmSource.clip = clip;
            _bgmSource.Play();
        }
        
    }
    
    public void PlaySE(string seKey, float volume = 1f)
    {
        AudioClip clip = audioSO.SESounds.Find(s => s.SoundName == seKey)?.Clip;
        if (clip == null) return;

        AudioSource source = GetAvailable2DSource();
        source.PlayOneShot(clip, volume);
    }

    public void PlayJingle(string jingleKey, float volume = 1)
    {
        AudioClip clip = audioSO.JingleSounds.Find(s => s.SoundName == jingleKey)?.Clip;
        if (clip == null) return;

        AudioSource source = GetAvailable2DSource();
        source.PlayOneShot(clip, volume);
    }

    public void StopBGM(float fadeTime = 0f)
    {
        _bgmFadCTS?.Cancel();
        if (fadeTime > 0f)
        {
            _bgmFadCTS = new CancellationTokenSource();
            FadeOutBGM(fadeTime, _bgmFadCTS).Forget();
        }
        else
        {
            _bgmSource.Stop();
        }
    }
    
    readonly Dictionary<string, AudioSource> _loopSESources = new();

    public void PlayLoopSE(string seKey, float volume = 1f)
    {
        AudioClip clip = audioSO.SESounds.Find(s => s.SoundName == seKey)?.Clip;
        if (clip == null) return;
        
        //同じクリップがもう再生されている場合はリターン
        if (_loopSESources.TryGetValue(seKey, out var existing) && existing != null)  
        {   
            return;
        }
        
        AudioSource source = GetAvailable2DSource();
        source.Stop();
        source.loop = true;
        source.clip = clip;
        source.volume = volume;
        source.Play();
        
        _loopSESources[seKey] = source;
    }

    public void StopLoopSE(string seKey)
    {
        if (!_loopSESources.TryGetValue(seKey, out var source) || source == null) return;
        
        source.Stop();
        source.loop = false;
        source.clip = null;
        _loopSESources.Remove(seKey);
    }

    public void SetVolume(AudioCategory category, float volume)
    {
        string paramName = $"{category}Volume";
        audioMixer.SetFloat(paramName, VolumeToDb(volume));

        switch (category)
        {
            case AudioCategory.BGM:
                PlayBGM("BGMTest1").Forget();
                break;
        }
    }
    public float GetVolume(AudioCategory category)
    {
        string paramName = $"{category}Volume";
        return audioMixer.GetFloat(paramName, out float volume) ? volume: 0f;
    }


    AudioSource GetAvailable2DSource()
    {
        foreach (var source in _seSources)
        {
            if (!source.isPlaying)
                return source;
        }
        
        var newSource = CreateAudioSource("SE", false);
        _seSources.Add(newSource);
        return newSource;
    }
    
    //BGMをフェードアウトさせる
    async UniTask FadeOutBGM(float duration,CancellationTokenSource cts,float fadeEndVolume = 0f)
    {
        if (audioMixer.GetFloat("BGMVolume", out float mixerVolume) == false)
        {
            throw new Exception("BGMVolumeパラメータが見つかりません");
        }
        try
        {
            _bgmSource.DOKill();
            _bgmSource.DOFade(fadeEndVolume, duration);
        
            await UniTask.Delay((int)(duration * 1000), cancellationToken: cts.Token);
        }
        finally
        {
            _bgmSource.Stop();
            _bgmSource.volume = mixerVolume;
        }
    }

    float VolumeToDb(float volume)
    {
        Debug.Log($"VolumeToDbの計算結果: { 20f * Mathf.Log10(volume) }");
        return volume > 0 ? 20f * Mathf.Log10(volume) : -80f;
    }
    void OnDestroy()
    {
        _bgmFadCTS?.Cancel();
    }
}
}