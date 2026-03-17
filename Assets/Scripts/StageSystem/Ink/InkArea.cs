using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace StageSystem.Ink
{
public interface IInkArea
{
    void  CreateInkArea(List<Vector2> points, IInkEffect effect,Material material);
}
/// <summary> インクエリアのオブジェクトにつけられる </summary>
public class InkArea : MonoBehaviour, IInkArea
{
    [SerializeField] MeshFilter meshFilter;
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] MeshCollider meshCollider;
    
    const float InkAreaActiveTime = 10f;
    
    Mesh _mesh;

    void Reset()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();
    }

    public void CreateInkArea(List<Vector2> points, IInkEffect effect,Material material)
    {
        _mesh = InkAreaMeshFactory.Create(points.ToArray());
        meshFilter.mesh = _mesh;
        meshCollider.sharedMesh = _mesh;
        meshRenderer.material = material;
        
        DestroyInkArea().Forget();
    }

    public string MaterialName { get; }

    async UniTask DestroyInkArea()
    {
        await UniTask.Delay((int)(InkAreaActiveTime * 1000));
        //todo エフェクト
        Destroy(gameObject);
    }
    
}
}
  
  
