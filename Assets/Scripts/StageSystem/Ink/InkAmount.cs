using System;
using InputSystemActions;
using R3;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace StageSystem.Ink
{
public interface IInkAmount
{
    float Ink { get; }
    bool IsInkAvailable();
    void RecoverInk();
    void ConsumeInk();
}
public class InkAmount : MonoBehaviour,IInkAmount
{
    [SerializeField] float recoverInkUsageSecond = 10f;
    [SerializeField] float consumeInkUsage = 1f;
    
    [Inject]
    public void Construct(ICurrentInkEffect currentInkEffect)
    {
        currentInkEffect.Get.Subscribe(effect =>
        {
            consumeInkUsage = effect.EffectUsageRate;
        }).AddTo(this);
    }
    
    bool _isHolding;
    InputActions  _inputActions;

    public float Ink { get; private set; } = 100;
    public bool IsInkAvailable() => Ink > 0;
    
    public void RecoverInk()
    {
        if (Ink >= 100 || _isHolding) return;
        Ink += recoverInkUsageSecond * Time.deltaTime;
    }

    public void ConsumeInk()
    {
        if (Ink <= 0) return;
        Ink -= consumeInkUsage;
    }

    void OnEnable()
    {
        _inputActions = new InputActions();
        _inputActions.Player.Enable();
        _inputActions.Player.Attack.started += OnClickStart;
        _inputActions.Player.Attack.canceled += OnClickEnd;
    }

    void OnClickStart(InputAction.CallbackContext context)
    { 
        _isHolding = true;
    }

    void OnClickEnd(InputAction.CallbackContext context)
    {
        _isHolding = false;
    }

    void OnDisable()
    {
        _inputActions.Player.Attack.started -= OnClickStart;
        _inputActions.Player.Attack.canceled -= OnClickEnd;
        _inputActions.Disable();
    }
}
}