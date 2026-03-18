using System;
using System.Collections.Generic;
using UnityEngine;

namespace StageSystem.CheckPoint
{
public interface ICheckPointManager
{
    public ICheckPoint CurrentCheckPoint { get; }
    public void ThroughCheckPoint(ICheckPoint checkPoint);
    public void MoveCheckPoint(Transform transform);
    
}
public class CheckPointManager  : MonoBehaviour, ICheckPointManager
{
      List<ICheckPoint>  _checkPoints = new();

      public ICheckPoint CurrentCheckPoint { get; private set; }
      
      

      void Reset()
      {

      }

      void Awake()
      {
          _checkPoints.Clear();
          transform.GetChild(0).GetComponentsInChildren(true, _checkPoints);
          Debug.Log($"CheckPointManager: Found {_checkPoints.Count} checkpoints in children.");
          _checkPoints.ForEach(checkPoint =>
          {
              checkPoint.Initialize(this);
          });
      }
      

      public void ThroughCheckPoint(ICheckPoint checkPoint)
      {
          Debug.Log($"Through checkpoint: {checkPoint.Transform.name}");
          CurrentCheckPoint = checkPoint;
      }

      public void MoveCheckPoint(Transform transform)
      {
          transform.position = CurrentCheckPoint.Transform.position;
      }
}
}