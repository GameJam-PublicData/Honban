using System.Collections.Generic;
using UnityEngine;

namespace StageSystem.Ink.Inks
{
public class NoGravityInkEffect  : IInkEffect
{
    readonly Dictionary<Rigidbody2D, Vector2> _originalVelocities = new();
    public string MaterialName => "NoGravityInkMaterial";
    
    public void UpdateInkArea(Rigidbody2D body)
    {
        Debug.Log("重力なしインクエリアの更新");
        body.MovePosition(body.position + _originalVelocities[body] * Time.deltaTime);
    }

    public void StartInkArea(Rigidbody2D body)
    {
        Debug.Log("重力なしインクエリアの開始");

        _originalVelocities[body] = body.linearVelocity;
    }

    public void StopInkArea(Rigidbody2D rigidbody)
    {
        Debug.Log("重力なしインクエリアの終了");
        _originalVelocities.Remove(rigidbody);
    }
}
}