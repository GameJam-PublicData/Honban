using System;
using System.Collections.Generic;
using UnityEngine;

namespace StageSystem.Area
{
    public interface IStrokeBuilder
    {
        bool IsCrossing(Vector2 worldPos, Action<List<Vector2>> historyPoints, out List<Vector2> areaPoints);
        void Clear();
    }

    public class StrokeBuilder : IStrokeBuilder
    {
        readonly List<Vector2> _points = new();
        const float MinDistance = 0.05f;

        public bool DistanceCheck(Vector2 worldPos)
        {
            return _points.Count > 0 && Vector2.Distance(_points[^1], worldPos) < MinDistance;
        }
        
        public bool IsCrossing(Vector2 worldPos, Action<List<Vector2>> historyPoints, out List<Vector2> areaPoints)
        {
            areaPoints = _points;

            // 直前で記録したポイントとの距離を計算しmin値以上か判別
            if (DistanceCheck(worldPos)) return false;

            _points.Add(worldPos);
            historyPoints?.Invoke(_points);
            // Debug.Log($"ポイント追加: {worldPos}, 総ポイント数: {_points.Count}");
            if (_points.Count < 4) return false;
            
            var latestStart = _points[^2];
            var latestEnd = _points[^1];

            for (int i = 0; i < _points.Count - 3; i++)
            {
                var start = _points[i];
                int startIndex = i;
                var end = _points[i + 1];
                
                if (Intersect(latestStart, latestEnd, start, end))
                {
                    Debug.Log("交差を検出");
                    //現在と一つ前の線分(last~)とfor文で現在のポイントをチェックしている
                    //StartとEndの線分が交差しているか
                    _points.RemoveRange(0,startIndex);//Startまでを消す
                    areaPoints = _points;
                    return true;
                }
            }
            return false;
        }

        public void Clear() => _points.Clear();
        
        static bool Intersect(Vector2 startPoint1, Vector2 endPoint1, Vector2 startPoint2, Vector2 endPoint2)
        {
            Vector2 v1 = endPoint1 - startPoint1;
            Vector2 v2 = endPoint2 - startPoint2;

            return Cross(v1, startPoint2 - startPoint1) * Cross(v1, endPoint2 - startPoint1) < 0 &&
                   Cross(v2, startPoint1 - startPoint2) * Cross(v2, endPoint1 - startPoint2) < 0;
        }

        static float Cross(Vector2 u, Vector2 v) => u.x * v.y - u.y * v.x;
    }
}