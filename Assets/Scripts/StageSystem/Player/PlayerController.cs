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
        private Rigidbody2D _rb;

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
        }

        void Update()
        {
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