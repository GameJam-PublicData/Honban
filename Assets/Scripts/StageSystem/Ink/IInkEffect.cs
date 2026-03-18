using R3;
using UnityEngine;

namespace StageSystem.Ink
{

/// <summary> インクの特殊効果 </summary>
public interface IInkEffect
{
    //どうする？
    void UpdateInkArea(Rigidbody2D body);
    void StartInkArea(Rigidbody2D  body);
    void StopInkArea(Rigidbody2D rigidbody);
    

    string MaterialName { get; }
}
}