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
    [SerializeField]int inkUsage = 1;
    bool _isHolding;
    InputActions  _inputActions;

    public float Ink { get; private set; } = 100;
    public bool IsInkAvailable() => Ink > 0;

    Vector2 _mousePos = Vector2.zero;
    
    void Update()
    {
        bool isMouseMove = _mousePos != Mouse.current.position.ReadValue();
        _mousePos  = Mouse.current.position.ReadValue();
        
        if (_isHolding && Ink > 0 && isMouseMove)
        {
            Ink -= inkUsage * Time.deltaTime;
            if(Ink < 0) Ink = 0;
        }
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