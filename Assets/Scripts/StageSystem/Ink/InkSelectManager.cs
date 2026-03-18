using System.Collections.Generic;
using InputSystemActions;
using R3;
using StageSystem.Ink.Inks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StageSystem.Ink
{
public interface ICurrentInkEffect
{
    ReadOnlyReactiveProperty<IInkEffect> Get { get; }
}
public class InkSelectManager : MonoBehaviour ,ICurrentInkEffect
{
    InputActions _inputActions;

    ReactiveProperty<IInkEffect> _currentInkEffect = new();
    public ReadOnlyReactiveProperty<IInkEffect> Get  => _currentInkEffect;

    readonly List<IInkEffect>  _inkEffects = new();
    int _currentIndex = 0;
    
    

    void InkEffectsInitialize()
    {
        //ステージのInkEffectを追加する
        //todo最終的にはsextsuteidekiruyouni
        //_inkEffects.Add(new NoGravityInkEffect());
        _inkEffects.Add(new AntiGravityInkEffect());
        _inkEffects.Add(new LowGravityEffect());
        _currentInkEffect.Value = _inkEffects[_currentIndex];
        _currentInkEffect.Subscribe(effect =>
        {
            Debug.Log($"Current Ink Effect: {effect.GetType()}");
        }).AddTo(this);
    }
    
    
    void OnEnable()
    {
        _inputActions = new InputActions();
        _inputActions.Player.Enable();
        _inputActions.Player.SwitchInk.started += SwitchInk;
        InkEffectsInitialize();
    }

    void SwitchInk(InputAction.CallbackContext ctx)
    {
        // インクの切り替え
        _currentIndex = (_currentIndex + 1) % _inkEffects.Count;
        _currentInkEffect.Value  = _inkEffects[_currentIndex];
        
    }
    
    void OnDisable()
    {
        _inputActions.Player.SwitchInk.started -= SwitchInk;
        _inputActions.Player.Disable();
    }

    
}
}