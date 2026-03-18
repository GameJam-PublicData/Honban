using System;
using UnityEngine;
using DG.Tweening;


namespace StageSystem.Ink
{
public class InkAntiGravity : MonoBehaviour,IInkEffect
{
    public void UpdateInkArea(Rigidbody2D rigidbody)
    {
        rigidbody.gravityScale = -1;
        //回転
        rigidbody.DORotate(180, 0.1f).SetEase(Ease.Linear);
    }

    public void StartInkArea(Rigidbody2D rigidbody)
    {
        
    }

    public void StopInkArea(Rigidbody2D rigidbody)
    {
        rigidbody.gravityScale = 1;
        rigidbody.DORotate(0, 0.1f).SetEase(Ease.Linear);
    }
}
}
