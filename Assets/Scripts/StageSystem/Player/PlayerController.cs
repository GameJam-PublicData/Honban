using InputSystemActions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StageSystem.Player
{
    public class PlayerController : MonoBehaviour
    {
        InputActions _inputActions;
        InputAction _moveAction;

        [SerializeField] float moveSpeed = 3f;
        [SerializeField] Vector2 speedUpMultiplier = Vector2.one;
        Rigidbody2D _rb;

        public Vector2 SpeedUpMultiplier => speedUpMultiplier;

        Vector2 _force;

        public void MultiplySpeedUpMultiplier(float multiplier)
        {
            Debug.Log($"SpeedUpMultiplier multiplied by {multiplier}");
            Debug.Log(Mathf.Max(0f, multiplier));
            speedUpMultiplier *= Mathf.Max(0f, multiplier);
        }

        public void RestoreSpeedUpMultiplier(Vector2 originalMultiplier)
        {
            speedUpMultiplier = originalMultiplier;
        }


        void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _inputActions = new InputActions();
            _inputActions.Player.Enable();
            _moveAction = _inputActions.Player.Move;
            
            GetComponent<IPlayerEffect>().OnInkEffect.Subscribe(effectData =>
            {
                if (effectData.effect is NoGravityInkEffect)
                {
                    _isNoGravity = effectData.isIn;
                }
            }).AddTo(this);
            
        }

        bool _isNoGravity = false;

        void Update()
        {
            if(_isNoGravity) return;
            Vector2 moveInput = _moveAction.ReadValue<Vector2>();
           
            //スピードアップ倍率を考慮して移動方向を計算
            Vector2 scaledInput = Vector2.Scale(moveInput, speedUpMultiplier);
            Vector2 moveDirection = transform.right * scaledInput.x + transform.up * scaledInput.y;

            //移動ベクトルを計算し
            Vector2 moveVector = _rb.linearVelocity;
            moveVector.x = moveDirection.x * moveSpeed;
            
            //反重力の場合操作を反転させる
            if(_rb.gravityScale < 0f) { moveVector.x = -moveVector.x; }
            
            //移動ベクトルをRigidbody2Dに適用
            _rb.linearVelocity = moveVector;
        }

        private void OnDestroy()
        {
            _inputActions.Player.Disable();
        }
    }
}