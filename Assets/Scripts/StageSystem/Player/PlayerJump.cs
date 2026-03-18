using InputSystemActions;
using StageSystem.Ink;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using VContainer;

namespace StageSystem
{
public class PlayerJump : MonoBehaviour
{
    InputActions _inputActions;
    InputAction _jumpAction;

    Rigidbody2D _rigidbody;
    Collider2D _collider;
    
    IInkAmount _inkAmount;

    [FormerlySerializedAs("_jumpForce")] [SerializeField]
    float jumpForce = 200;

    [FormerlySerializedAs("_gravity")] [SerializeField]
    Vector2 gravity = new Vector2(0, 1);

    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundCheckDistance = 0.1f;

    bool _isGround;

    //Vector2 _gravity;

    [Inject]
    void Construct(IInkAmount inkAmount)
    {
        _inkAmount = inkAmount;
    }

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
        _defaultJumpForce = jumpForce;
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
        gravity = new Vector2(0, _rigidbody.gravityScale);

        CheckGround();
    }
    
    
    void CheckGround()
    {
        Bounds bounds = _collider.bounds;
    
        // 重力方向によってい角度を変える
        float gravitySign = Mathf.Sign(_rigidbody.gravityScale);
        if (gravitySign == 0f)
        {
            _isGround = false;
            return;
        }
    
        Vector2 rayDirection = gravitySign > 0f ? Vector2.down : Vector2.up;
    
        //中心から、レイ方向側の面まで移動した位置を始点にする
        float halfExtentAlongRay =
            Mathf.Abs(rayDirection.x) * bounds.extents.x +
            Mathf.Abs(rayDirection.y) * bounds.extents.y;
    
        Vector2 origin = (Vector2)bounds.center + rayDirection * halfExtentAlongRay;
    
        RaycastHit2D hit = Physics2D.Raycast(origin, rayDirection, groundCheckDistance, groundLayer);
        _isGround = hit.collider != null;
        
        if (_isGround) _inkAmount.RecoverInk();
    
        Debug.DrawRay(origin, rayDirection * groundCheckDistance, _isGround ? Color.green : Color.red);
    }
    
    float _defaultJumpForce;
    
    public void SetJumpPower(float power)
    {
        jumpForce = _defaultJumpForce * power;
    }

    public void InitJumpPower()
    {
        jumpForce = _defaultJumpForce;
    }
    
}
}