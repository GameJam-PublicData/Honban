using System;
using UnityEngine;

namespace StageSystem.Ink
{
public class InkAntiGravity : MonoBehaviour,IInkEffect
{
    public void UpdateInkArea(Rigidbody2D rigidbody)
    {
        rigidbody.gravityScale = -1;
    }

    public void StartInkArea(Rigidbody2D rigidbody)
    {
        
    }

    public void StopInkArea(Rigidbody2D rigidbody)
    {
        rigidbody.gravityScale = 1;
    }
}
}
