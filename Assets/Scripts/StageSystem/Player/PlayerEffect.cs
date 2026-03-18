using StageSystem.Ink;
using UnityEngine;


public class PlayerEffect : MonoBehaviour
{     
    [SerializeField] private IInkEffect _inkEffect; 
    [SerializeField] private Rigidbody _rigidbody;
    private IInkEffect _inkEffectImplementation;
    private IInkEffect _doubleJumpEffect;

    void Update()
    {
        
    }
}
