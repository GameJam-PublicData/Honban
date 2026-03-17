using System.Collections.Generic;
using StageSystem.Ink;
using UnityEngine;
using VContainer;

namespace StageSystem
{
/// <summary>
/// プレイヤーが円を描いたらインクエリアを生成するクラス
/// </summary>
public class InkManager
{
    public InkManager([Key("InkAreaPrefab")]  GameObject inkAreaPrefab)
    {
        
    }


    public void CreateInkArea(List<Vector2> points,IInkEffect effect)
    {
        
    }

}
}