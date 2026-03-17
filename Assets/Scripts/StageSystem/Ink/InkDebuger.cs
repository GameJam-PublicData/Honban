using UnityEngine;

namespace StageSystem.Ink
{
public class InkDebuger : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        var inkArea = collision.gameObject.GetComponent<IInkEffect>();
        if (inkArea != null)
        {
            inkArea.StartInkArea(collision.rigidbody);
        }
    }
    
    void OnCollisionStay2D(Collision2D collision)
    {
        var inkArea = collision.gameObject.GetComponent<IInkEffect>();
        if (inkArea != null)
        {
            inkArea.UpdateInkArea(collision.rigidbody);
        }
    }
    
    void OnCollisionExit2D(Collision2D collision)
    {
        var inkArea = collision.gameObject.GetComponent<IInkEffect>();
        if (inkArea != null)
        {
            inkArea.StopInkArea(collision.rigidbody);
        }
    }
}
}
