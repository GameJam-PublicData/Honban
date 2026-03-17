using System.Collections.Generic;
using InputSystemActions;
using R3;
using StageSystem.Ink;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StageSystem
{
public class InkStageSelectManager : MonoBehaviour
{
    InputActions _inputActions;

    public ReactiveProperty<IInkEffect> CurrentInkEffect;

    List<IInkEffect>  _inkEffects = new();
    int _currentIndex = 0;

    void InkEffectsInitialize()
    {
        //ステージのInkEffectを追加する
        //todo最終的にはsextsuteidekiruyouni
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
        
        CurrentInkEffect.Value  = _inkEffects[_currentIndex];
    }

    void BackInk(InputAction.CallbackContext ctx)
    {
        _currentIndex--;
        if (_currentIndex < 0)
        {
            _currentIndex = _inkEffects.Count - 1;
        }
        CurrentInkEffect.Value  = _inkEffects[_currentIndex];
    }

    void OnDisable()
    {
        _inputActions.Player.NextInk.started -= NextInk;
        _inputActions.Player.BackInk.started -= BackInk;
        _inputActions.Player.Disable();
    }
    
    
    
    
}
}