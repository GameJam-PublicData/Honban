using System;
using InputSystemActions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

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
    [SerializeField] float maxInkAmount = 200f;
    
    bool _isHolding;
    InputActions  _inputActions;

    [field: SerializeField] public float Ink { get; private set; } = 100;
    public bool IsInkAvailable() => Ink > 0;
    
    public void RecoverInk()
    {
        if (Ink >= maxInkAmount || _isHolding) return;
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