using UnityEngine;

namespace StageSystem.Ink
{

/// <summary> インクの特殊効果 </summary>
public interface IInkEffect
{
    //どうする？
    void UpdateInkArea(Rigidbody rigidbody);
    void StartInkArea(Rigidbody  rigidbody);
    void StopInkArea(Rigidbody rigidbody);
}
}