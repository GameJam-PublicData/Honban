using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace StageSystem.Ink
{
public interface IInkArea
{
    void  CreateInkArea(List<Vector2> points, IInkEffect effect);
}
/// <summary> インクエリアのオブジェクトにつけられる </summary>
public class InkArea : MonoBehaviour, IInkArea
{
    [SerializeField] MeshFilter meshFilter;
    
    const float InkAreaActiveTime = 10f;
    
    Mesh _mesh;

    void Reset()
    {
        meshFilter = GetComponent<MeshFilter>();
    }

    public void CreateInkArea(List<Vector2> points, IInkEffect effect)
    {
        _mesh = new Mesh();
        //pointsに沿った形のMeshを作る
    }

    async UniTask DestroyInkArea()
    {
        await UniTask.Delay((int)(InkAreaActiveTime * 1000));
        //todo エフェクト
        Destroy(gameObject);
    }
}
}
  
  
