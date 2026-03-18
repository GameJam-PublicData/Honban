using System.Collections.Generic;
using UnityEngine;

namespace StageSystem.Ink.Inks
{
public class NoGravityInkEffect  : IInkEffect
{
    readonly Dictionary<Rigidbody2D, Vector2> _originalVelocities = new();
    public string MaterialName => "NoGravityInkMaterial";
    
    public void UpdateInkArea(Rigidbody2D rigidbody)
    {
        Debug.Log("重力なしインクエリアの更新");
        rigidbody.MovePosition(rigidbody.position + _originalVelocities[rigidbody] * Time.deltaTime);
    }

    public void StartInkArea(Rigidbody2D rigidbody)
    {
        Debug.Log("重力なしインクエリアの開始");

        _originalVelocities[rigidbody] = rigidbody.linearVelocity;
    }

    public void StopInkArea(Rigidbody2D rigidbody)
    {
        Debug.Log("重力なしインクエリアの終了");
        _originalVelocities.Remove(rigidbody);
    }


}
}