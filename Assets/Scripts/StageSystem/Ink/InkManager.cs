using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace StageSystem.Ink
{
public interface IInkManager
{
    void CreateInkArea(List<Vector2> points);
}
/// <summary>
/// プレイヤーが円を描いたらインクエリアを生成するクラス
/// </summary>
public class InkManager : MonoBehaviour , IInkManager
{
    [SerializeField] GameObject inkAreaPrefab;
    Transform _inkAreaRootParent;
    ICurrentInkEffect _currentInkEffect;

    void Start()
    {
        _inkAreaRootParent = new GameObject("InkAreaRoot").transform;
    } 

    [Inject]
    public void Construct(ICurrentInkEffect currentInkEffect)
    {
        _currentInkEffect = currentInkEffect;
    }

    //ここでインク作成依頼
    public void CreateInkArea(List<Vector2> points)
    {
        IInkEffect inkEffect =  _currentInkEffect.Get.CurrentValue;
        GameObject inkAreaObj = Instantiate(inkAreaPrefab, _inkAreaRootParent);
        IInkArea inkArea = inkAreaObj.GetComponent<IInkArea>(); 
        inkArea.CreateInkArea(points, inkEffect);
    }

}
}