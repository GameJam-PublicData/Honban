using DG.Tweening;
using UnityEngine;

namespace StageSystem.Ink.Inks
{
public class AntiGravityInkEffect : IInkEffect
{
    // TODO: MaterialNameが実装されていないため、仮で実装なので変更が加える必要があるなら変更してください。
    string IInkEffect.MaterialName => "AntiGravity";
    
    public void UpdateInkArea(Rigidbody2D rigidbody)
    {
        if (rigidbody.gravityScale > 0)
        {
            rigidbody.gravityScale *= -1f;
            //回転
            rigidbody.DORotate(180, 0.1f).SetEase(Ease.Linear);
        }
    }

    public void StartInkArea(Rigidbody2D rigidbody)
    {
        
    }

    public void StopInkArea(Rigidbody2D rigidbody)
    {
        if (rigidbody.gravityScale < 0)
        {
            rigidbody.gravityScale = Mathf.Abs(rigidbody.gravityScale);
            //回転
            rigidbody.DORotate(0, 0.1f).SetEase(Ease.Linear);
        }
    }
}
}
