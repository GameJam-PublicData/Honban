using UnityEngine;

namespace StageSystem.Ink
{
public class InkDebuger : MonoBehaviour
{
    Rigidbody2D _rigidbody;
    
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        var inkEffect = other.GetComponent<IInkEffect>();
        if (inkEffect != null)
        {
            inkEffect.StartInkArea(_rigidbody);
        }

    }
    
    void OnTriggerStay2D(Collider2D other)
    {
        var inkEffect = other.GetComponent<IInkEffect>();
        if (inkEffect != null)
        {
            inkEffect.UpdateInkArea(_rigidbody);
        }
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        var inkEffect = other.GetComponent<IInkEffect>();
        if (inkEffect != null)
        {
            inkEffect.StopInkArea(_rigidbody);
        }
    }
}
}
