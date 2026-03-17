using UnityEngine;

namespace StageSystem.Ink
{

/// <summary> インクの特殊効果 </summary>
public interface IInkEffect
{
    //どうする？
    void UpdateInkArea(Rigidbody2D rigidbody);
    void StartInkArea(Rigidbody2D  rigidbody);
    void StopInkArea(Rigidbody2D rigidbody);

    string MaterialName { get; }
}
}