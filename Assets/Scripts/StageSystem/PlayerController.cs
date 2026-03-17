using System;
using UnityEngine;
using UnityEngine.InputSystem;
using InputSystemActions;

public class PlayerController : MonoBehaviour
{
    InputActions _inputActions;
    InputAction _moveAction;
    [SerializeField] float moveSpeed;
    private Rigidbody _rb;


    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _inputActions = new InputActions();
        _inputActions.Player.Enable();
        _moveAction = _inputActions.Player.Move;
        
    }

    void Update()
    {
        Vector3 moveValue = _moveAction.ReadValue<Vector2>();
        moveValue = new Vector3(moveValue.x, 0, moveValue.y);


        Vector3 moveDirection = transform.TransformDirection(moveValue);
        _rb.MovePosition(_rb.position + moveDirection * (moveSpeed * Time.deltaTime));
    }

    private void OnDestroy()
    {
        _inputActions.Player.Disable();
    }
}