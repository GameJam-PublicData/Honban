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

        [SerializeField] float moveSpeed = 3f;
        [SerializeField] Vector2 speedUpMultiplier = Vector2.one;
        Rigidbody2D _rb;

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