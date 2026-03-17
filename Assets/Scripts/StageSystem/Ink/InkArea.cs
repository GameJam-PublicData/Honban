using System;
using System.Collections.Generic;
using StageSystem.Ink;
using UnityEngine;

namespace StageSystem
{
public interface IInkArea
{
    void  CreateInkArea(List<Vector2> points, IInkEffect effect);
}
/// <summary> インクエリアのオブジェクトにつけられる </summary>
public class InkArea : MonoBehaviour, IInkArea
{
    [SerializeField] MeshFilter meshFilter;
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
}
}
  
  
