using System.Collections.Generic;
using UnityEngine;

namespace StageSystem.Ink.Inks
{ 
public class AntiGravityInkEffect : IInkEffect
{
    readonly Dictionary<Rigidbody2D, Vector2> _originalVelocities = new();
    private IInkEffect _inkEffectImplementation;

    public void UpdateInkArea(Rigidbody2D body)
    {
        Debug.Log("反重力インクエリアの更新");
        body.MovePosition(body.position + -_originalVelocities[body] * Time.deltaTime);
    }

    public void StartInkArea(Rigidbody2D body)
    { 
        Debug.Log("反重力インクエリアの開始");
        _originalVelocities[body] = body.linearVelocity;
    }

    public void StopInkArea(Rigidbody2D rigidbody)
    {
        Debug.Log("反重力インクエリアの終了");
        _originalVelocities.Remove(rigidbody);
    }

    public string MaterialName => _inkEffectImplementation.MaterialName;
}
}
