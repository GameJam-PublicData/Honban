using System;
using UnityEngine;

namespace Utils.Test
{
public class MeshTest : MonoBehaviour
{
    void Start()
    {
        Vector2[] points = new Vector2[]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(1, 1),
            new Vector2(0, 1)
        };
        GetComponent<MeshFilter>().mesh = StageSystem.Ink.InkAreaMeshFactory.Create(points);
    }
}
}
  
  
