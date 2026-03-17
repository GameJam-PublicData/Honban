using UnityEngine;
using UnityEngine.InputSystem;

namespace StageSystem.Ink
{
    public class InkUse : MonoBehaviour
    {
        public bool inkReady = true;
        public float ink = 100;
        private int _useInk = 1;
        public bool isHolding;

        void Update()
        {
            if (isHolding && ink > 0)
            {
                ink = Mathf.Max(ink, 0);
                ink -= _useInk * Time.deltaTime;
            }
            
            if(ink <= 0)
            {
                inkReady = false;
            }
        }

        public void OnClick(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                isHolding = true;
            }
            else if (context.canceled)
            {
                isHolding = false;
            }
        }
    }
}
