using System;
using InputSystemActions;
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

        public void MultiplySpeedUpMultiplier(float multiplier)
        {
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
            _inputActions = new InputActions();
            _inputActions.Player.Enable();
            _moveAction = _inputActions.Player.Move;
        }

        void Update()
        {
            Vector2 moveInput = _moveAction.ReadValue<Vector2>();
           
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
                    playerSprite.transform.localScale = new Vector3(-MathF.Abs(playerSprite.transform.localScale.x),
                        playerSprite.transform.localScale.y, 1f);
                }
                else if (moveInput.x < 0)
                {
                    playerSprite.transform.localScale = new Vector3(MathF.Abs(playerSprite.transform.localScale.x),
                        playerSprite.transform.localScale.y, 1f);
                }
            }

            Vector2 scaledInput = Vector2.Scale(moveInput, speedUpMultiplier);
            Vector2 moveDirection = transform.right * scaledInput.x + transform.up * scaledInput.y;
            
            Vector2 moveVector = _rb.linearVelocity;
            moveVector.x = moveDirection.x * moveSpeed;
            _rb.linearVelocity = moveVector;
        }

        private void OnDestroy()
        {
            _inputActions.Player.Disable();
        }
    }
}