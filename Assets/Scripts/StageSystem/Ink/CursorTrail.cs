using System.Collections.Generic;
using UnityEngine;

namespace StageSystem.UI.Mouse
{
    public interface ICursorTrail
    {
        void Draw(List<Vector2> points);
    }
    public class CursorTrail :  MonoBehaviour, ICursorTrail
    {
        LineRenderer _lineRenderer;
        Camera _mainCamera;

        void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>() ?? gameObject.AddComponent<LineRenderer>();
            
            _lineRenderer.useWorldSpace = false;
            _lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            
            _lineRenderer.widthMultiplier = 0.1f;
            
            _mainCamera = Camera.main;
            if (_mainCamera == null)
                Debug.LogError("Main Camera not found.");
        }
        
        public void Draw(List<Vector2> points)
        {
            _lineRenderer.positionCount = points.Count;
            for (int i = 0; i < points.Count; i++)
            {
                Vector3 worldPos = new Vector3(points[i].x, points[i].y, 0f);
                Vector3 localPos = transform.TransformPoint(worldPos);
                _lineRenderer.SetPosition(i, localPos);
            }
        }
    }
}