using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Profiling;

namespace StageSystem.UI.Mouse
{
    public interface ICursorTrail
    {
        void Draw(List<Vector2> points);
        void ChangeColor(Color color);
        void FadeOut();
    }
    public class CursorTrail :  MonoBehaviour, ICursorTrail
    {
        LineRenderer _lineRenderer;
        UnityEngine.Camera _mainCamera;
        Material _lineMaterial;

        void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>() ?? gameObject.AddComponent<LineRenderer>();
            
            var shader = Shader.Find("Sprites/Default");
            if (shader != null)
            {
                _lineMaterial = new Material(shader);
                _lineRenderer.material = _lineMaterial;
            }
            else
            {
                Debug.LogWarning("Shader 'Sprites/Default' not found.");
            }
            
            _mainCamera = UnityEngine.Camera.main;
            if (_mainCamera == null)
                Debug.LogError("Main Camera not found.");

            Reset();
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

        public void ChangeColor(Color color)
        {
            _lineRenderer.startColor = color;
            _lineRenderer.endColor = color;
        }
        
        public void FadeOut()
        {
            var mat = _lineRenderer.material;
            mat.DOColor(new Color(1, 1, 1, 0), "_Color", 0.5f).onComplete = Reset;
        }

        void Reset()
        {
            transform.position = Vector3.zero;
            
            _lineRenderer.useWorldSpace = false;
            if (_lineMaterial != null)
            {
                _lineRenderer.material = _lineMaterial;
                _lineMaterial.color = Color.white;
            }
            
            _lineRenderer.widthMultiplier = 0.1f;
            _lineRenderer.positionCount = 0;
        }
    }
}