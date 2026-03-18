using System.Linq;
using UnityEngine;

namespace StageSystem.Ink
{
using System.Collections.Generic;
using UnityEngine;
public static class InkAreaMeshFactory
{
    public static Mesh Create(Vector2[] points)
    { 
       return BuildMesh(points);
    }
    static Mesh BuildMesh(Vector2[] points)
    {
        if (points == null || points.Length < 3)
        {
            Debug.LogWarning("点が3つ以上必要です");
            return null;
        }

        Mesh mesh = new Mesh();
        mesh.name = "PolygonMesh";
        
        Vector3[] vertices = points.Select(p => new Vector3(p.x, p.y, 0f)).ToArray();

        // Ear Clipping で三角形インデックスを生成
        int[] triangles = Triangulate(points);
        
        mesh.vertices  = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        return mesh;
    }

    // ---- Ear Clipping 三角形分割 ----

    static int[] Triangulate(Vector2[] polygon)
    {
        var indices = new List<int>();
        var remaining = new List<int>();
        for (int i = 0; i < polygon.Length; i++)
            remaining.Add(i);

        // 時計回りに統一（反時計回りなら反転）
        if (SignedArea(polygon) > 0)
            remaining.Reverse();

        int safety = polygon.Length * polygon.Length;
        while (remaining.Count > 3 && safety-- > 0)
        {
            bool earFound = false;
            for (int i = 0; i < remaining.Count; i++)
            {
                int a = remaining[(i - 1 + remaining.Count) % remaining.Count];
                int b = remaining[i];
                int c = remaining[(i + 1) % remaining.Count];

                if (IsEar(polygon, remaining, a, b, c))
                {
                    indices.Add(a);
                    indices.Add(b);
                    indices.Add(c);
                    remaining.RemoveAt(i);
                    earFound = true;
                    break;
                }
            }
            if (!earFound) break; // 自己交差などで分割不可
        }

        // 最後の三角形
        if (remaining.Count == 3)
        {
            indices.Add(remaining[0]);
            indices.Add(remaining[1]);
            indices.Add(remaining[2]);
        }

        return indices.ToArray();
    }

    static bool IsEar(Vector2[] poly, List<int> remaining, int a, int b, int c)
    {
        // 耳（凸頂点）かどうか
        if (Cross(poly[a], poly[b], poly[c]) >= 0) return false;

        // 他の頂点が三角形内に入っていないか
        foreach (int idx in remaining)
        {
            if (idx == a || idx == b || idx == c) continue;
            if (PointInTriangle(poly[idx], poly[a], poly[b], poly[c]))
                return false;
        }
        return true;
    }

    static float Cross(Vector2 o, Vector2 a, Vector2 b)
        => (a.x - o.x) * (b.y - o.y) - (a.y - o.y) * (b.x - o.x);

    static float SignedArea(Vector2[] poly)
    {
        float area = 0f;
        for (int i = 0; i < poly.Length; i++)
        {
            var a = poly[i];
            var b = poly[(i + 1) % poly.Length];
            area += (a.x * b.y - b.x * a.y);
        }
        return area * 0.5f;
    }

    static bool PointInTriangle(Vector2 p, Vector2 a, Vector2 b, Vector2 c)
    {
        float d1 = Cross(a, b, p);
        float d2 = Cross(b, c, p);
        float d3 = Cross(c, a, p);
        bool hasNeg = d1 < 0 || d2 < 0 || d3 < 0;
        bool hasPos = d1 > 0 || d2 > 0 || d3 > 0;
        return !(hasNeg && hasPos);
    }
}
}