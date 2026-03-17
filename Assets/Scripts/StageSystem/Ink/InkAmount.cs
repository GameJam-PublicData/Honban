using System;
using InputSystemActions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StageSystem.Ink
{
public interface IInkAmount
{
    float Ink { get; }
    bool IsInkAvailable();
}
public class InkAmount : MonoBehaviour,IInkAmount
{
    int _inkUsage = 1;
    bool _isHolding;
    InputActions  _inputActions;

    public float Ink { get; private set; } = 100;
    public bool IsInkAvailable() => Ink > 0;

    void Update()
    {
        if (_isHolding && Ink > 0)
        {
            Ink -= _inkUsage * Time.deltaTime;
            if(Ink < 0) Ink = 0;
        }
    }

    void OnEnable()
    {
        _inputActions = new InputActions();
        _inputActions.Player.Enable();
        _inputActions.Player.Attack.performed += OnClick;
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _isHolding = true;
        }
        else if (context.canceled)
        {
            _isHolding = false;
        }
    }

    void OnDisable()
    {
        _inputActions.Player.Attack.performed -= OnClick;
        _inputActions.Disable();
    }
}
}