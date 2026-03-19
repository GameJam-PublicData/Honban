using System;
using InputSystemActions;
using R3;
using StageSystem.Ink.Inks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StageSystem.Player
{
    public class PlayerController : MonoBehaviour
    {
        InputActions _inputActions;
        InputAction _moveAction;
    
        [SerializeField] float moveSpeed;
        [SerializeField] private Vector2 speedUpMultiplier;
        [SerializeField] GameObject playerSprite;
        
        private Rigidbody2D _rb;
        PlayerAnimator _animator;
        
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
            _animator = GetComponent<PlayerAnimator>();
            
            GetComponent<IPlayerEffect>().OnInkEffect.Subscribe(effectData =>
            {
                if (effectData.effect is NoGravityInkEffect)
                {
                    _isNoGravity = effectData.isIn;
                }
            }).AddTo(this);
        }

        void OnEnable()
        {
            _inputActions = new InputActions();
            _inputActions.Player.Enable();
            _moveAction = _inputActions.Player.Move;
        }

        void OnDisable()
        {
            _inputActions.Player.Disable();
        }

        bool _isNoGravity = false;

        void Update()
        {
            if(_isNoGravity) return;
            Vector2 moveInput = _moveAction.ReadValue<Vector2>();
           
            //スピードアップ倍率を考慮して移動方向を計算
            if(moveInput != Vector2.zero)
            {
                _animator.WalkStart();
            }
            else
            {
                _animator.WalkEnd();
            }
            
            //左に行くときは左向き　右の時は右向きにする
            if (playerSprite != null)
            {
                if (moveInput.x > 0)
                {
                    Vector3 scale = new Vector3(
                        -Mathf.Abs(playerSprite.transform.localScale.x), 
                        playerSprite.transform.localScale.y, 1f);

                    if (_rb.gravityScale < 0f)
                    {
                        scale.x *= -1;
                    }
                    
                    playerSprite.transform.localScale = scale;
                }
                else if (moveInput.x < 0)
                {
                    Vector3 scale = new Vector3(
                        Mathf.Abs(playerSprite.transform.localScale.x), 
                        playerSprite.transform.localScale.y, 1f);

                    if (_rb.gravityScale < 0f)
                    {
                        scale.x *= -1;
                    }
                    
                    playerSprite.transform.localScale = scale;
                }
            }

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
        
    }
}