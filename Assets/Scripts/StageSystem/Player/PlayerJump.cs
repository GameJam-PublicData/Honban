using System.Collections;
using System.Collections.Generic;
using InputSystemActions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace StageSystem
{
public class PlayerJump : MonoBehaviour
{
    InputActions _inputActions;

    InputAction _jumpAction;

    Rigidbody2D _rigidbody;
    Collider2D _collider;

    [FormerlySerializedAs("_jumpForce")] [SerializeField]
    float jumpForce = 200;

    [FormerlySerializedAs("_gravity")] [SerializeField]
    Vector2 gravity = new Vector2(0, 1);

    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundCheckDistance = 0.1f;

    bool _isGround;

    //Vector2 _gravity;


    void OnEnable()
    {
        _inputActions = new InputActions();
        _inputActions.Player.Enable();
        _jumpAction = _inputActions.Player.Jump;

        //コールバック登録
        _jumpAction.performed += ctx => OnJumpStart();
        _jumpAction.canceled += ctx => OnJumpCancel();

        Debug.Log("完了");
    }

    void OnDisable()
    {
        _jumpAction.performed -= ctx => OnJumpStart();
        _jumpAction.canceled -= ctx => OnJumpCancel();

        _inputActions.Player.Disable();
        _inputActions.Dispose();
        _inputActions = null;
    }

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
    }

    void OnJumpStart()
    {
        if (!_isGround) return;
        _rigidbody.AddForce(gravity * jumpForce);
    }

    float _jumpCancelVelocityThreshold = 0.25f;
    
    void OnJumpCancel()
    {
        //まだ上昇を続けてるなら　反対のアドフォースをかける
        if (_rigidbody.linearVelocity.y > _jumpCancelVelocityThreshold && _rigidbody.gravityScale == 1 ||
            _rigidbody.linearVelocity.y < -_jumpCancelVelocityThreshold && _rigidbody.gravityScale == -1)
        {
            Debug.Log(_rigidbody.linearVelocity.y);
            _rigidbody.AddForce(gravity * -jumpForce / 3);
        }
        //_rigidbody.linearVelocity = Vector2.zero;
    }


    void FixedUpdate()
    {
        if (gravity.y == 1)
        {
            _rigidbody.gravityScale = 1;
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            _rigidbody.gravityScale = -1;
            transform.localRotation = Quaternion.Euler(0, 0, 180);
        }

        CheckGround();
    }


    void CheckGround()
    {
        Bounds bounds = _collider.bounds;
        Vector2 direction = -transform.up;
        float halfHeight = Vector2.Dot(
            bounds.extents,
            new Vector2(Mathf.Abs(transform.up.x), Mathf.Abs(transform.up.y))
        );
        Vector2 origin = (Vector2)bounds.center - (Vector2)transform.up * halfHeight;

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, groundCheckDistance, groundLayer);
        _isGround = hit.collider != null;

        Debug.DrawRay(origin, direction * groundCheckDistance, _isGround ? Color.green : Color.red);
    }
}
}