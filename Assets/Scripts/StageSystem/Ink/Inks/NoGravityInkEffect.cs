using System.Collections.Generic;
using UnityEngine;

namespace StageSystem.Ink.Inks
{
public class NoGravityInkEffect  : IInkEffect
{
    readonly Dictionary<Rigidbody, Vector3> _originalVelocities = new();
    
    public void UpdateInkArea(Rigidbody rigidbody)
    {
        Debug.Log("重力なしインクエリアの更新");
        rigidbody.MovePosition(rigidbody.position + _originalVelocities[rigidbody] * Time.deltaTime);
    }

    public void StartInkArea(Rigidbody rigidbody)
    {
        Debug.Log("重力なしインクエリアの開始");

        _originalVelocities[rigidbody] = rigidbody.linearVelocity;
    }

    public void StopInkArea(Rigidbody rigidbody)
    {
        Debug.Log("重力なしインクエリアの終了");
        _originalVelocities.Remove(rigidbody);
    }
}
}