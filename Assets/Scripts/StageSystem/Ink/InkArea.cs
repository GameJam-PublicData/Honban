using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using R3;
using Unity.VisualScripting;
using UnityEngine;

namespace StageSystem.Ink
{
public interface IInkArea
{
    void  CreateInkArea(List<Vector2> points, IInkEffect effect,Material material);
    
    IInkEffect GetInkEffect(Action onEffectEnd);
}
/// <summary> インクエリアのオブジェクトにつけられる </summary>
public class InkArea : MonoBehaviour, IInkArea
{
    [SerializeField] MeshFilter meshFilter;
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] PolygonCollider2D polygonCollider2D;
    
    const float InkAreaActiveTime = 10f;
    
    Action _onEffectEnd = () => { };
    
    IInkEffect  _inkEffect;
    public IInkEffect GetInkEffect(Action onEffectEnd)
    {
        _onEffectEnd += onEffectEnd;
        return _inkEffect;
    }
    
    
    Mesh _mesh;

    void Reset()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        polygonCollider2D = GetComponent<PolygonCollider2D>();
    }

    public void CreateInkArea(List<Vector2> points, IInkEffect effect,Material material)
    {
        _mesh = InkAreaMeshFactory.Create(points.ToArray());
        meshFilter.mesh = _mesh;
        polygonCollider2D.SetPath(0, points.ToArray());
        meshRenderer.material = material;
        
        _inkEffect = effect;
        
        DestroyInkArea().Forget();
    }
    

    async UniTask DestroyInkArea()
    {
        await UniTask.Delay((int)(InkAreaActiveTime * 1000));
        //todo エフェクト
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        _onEffectEnd?.Invoke();
    }
}
}
  
  
