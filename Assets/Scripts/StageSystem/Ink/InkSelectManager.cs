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

    ReactiveProperty<IInkEffect> _currentInkEffect;
    public ReadOnlyReactiveProperty<IInkEffect> Get  => _currentInkEffect;

    readonly List<IInkEffect>  _inkEffects = new();
    int _currentIndex = 0;

    void InkEffectsInitialize()
    {
        //ステージのInkEffectを追加する
        //todo最終的にはsextsuteidekiruyouni
        _inkEffects.Add(new NoGravityInkEffect());
    }
    
    
    void OnEnable()
    {
        _inputActions = new InputActions();
        _inputActions.Player.Enable();
        _inputActions.Player.NextInk.started += NextInk;
        _inputActions.Player.BackInk.started += BackInk;
        InkEffectsInitialize();
    }

    void NextInk(InputAction.CallbackContext ctx)
    {
        _currentIndex++;
        if (_currentIndex >= _inkEffects.Count)
        {
            _currentIndex = 0;
        }
        
        _currentInkEffect.Value  = _inkEffects[_currentIndex];
    }

    void BackInk(InputAction.CallbackContext ctx)
    {
        _currentIndex--;
        if (_currentIndex < 0)
        {
            _currentIndex = _inkEffects.Count - 1;
        }
        _currentInkEffect.Value  = _inkEffects[_currentIndex];
    }

    void OnDisable()
    {
        _inputActions.Player.NextInk.started -= NextInk;
        _inputActions.Player.BackInk.started -= BackInk;
        _inputActions.Player.Disable();
    }

    
}
}