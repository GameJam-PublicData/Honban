using System;
using System.Collections.Generic;
using MainSystem.Audio;
using R3;
using StageSystem.Ink;
using UnityEngine;
using VContainer;

namespace StageSystem.Player
{

public interface IPlayerEffect//これはGetComp
{
    Observable<(bool isIn,IInkEffect effect)> OnInkEffect { get; }
}

public class PlayerEffect : MonoBehaviour ,IPlayerEffect
{     
    Dictionary<IInkArea,IInkEffect> _currentEffects = new();

    Subject<(bool isIn,IInkEffect)> _inkEffectSubject = new();
    public Observable<(bool isIn,IInkEffect effect)> OnInkEffect => _inkEffectSubject;

    IAudioManager _audioManager;

    [Inject]
    void Construct(IAudioManager audioManager)
    {
        _audioManager = audioManager;
    }
    
    Rigidbody2D _rigidbody2D;
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out IInkArea inkArea))
        {
            IInkEffect effect = inkArea.GetInkEffect(() => EndEffect(inkArea));
            _currentEffects.Add(inkArea,effect);
            effect.StartInkArea(_rigidbody2D);
            _inkEffectSubject.OnNext((true,effect));
            
            Debug.Log("PlaySEAntiGravityStart");
            
            _audioManager.PlaySE("AntiGravityStart");
        }
    }

    void Update()
    {
        foreach (var effect in _currentEffects.Values)
        {
            effect.UpdateInkArea(_rigidbody2D);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.TryGetComponent(out IInkArea inkArea)) return;
        if (_currentEffects.ContainsKey(inkArea))
        {
            EndEffect(inkArea);
        }
    }

    void EndEffect(IInkArea inkArea)
    {
        if (_currentEffects.TryGetValue(inkArea, out var effect))
        {
            effect.StopInkArea(_rigidbody2D);
            _inkEffectSubject.OnNext((false,effect));
        }
        _currentEffects.Remove(inkArea);
    }
}
}
