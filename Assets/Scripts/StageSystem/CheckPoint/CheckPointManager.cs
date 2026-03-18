using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using StageSystem.Player;
using UnityEngine;

namespace StageSystem.CheckPoint
{
public interface ICheckPointManager
{
    public ICheckPoint CurrentCheckPoint { get; }
    public void ThroughCheckPoint(ICheckPoint checkPoint);
    public UniTask MoveCheckPoint(Transform transform);
    
}

public class CheckPointManager : MonoBehaviour, ICheckPointManager
{
    List<ICheckPoint> _checkPoints = new();
    [SerializeField] CheckPointBlackOutManager blackOutManager;

    public ICheckPoint CurrentCheckPoint { get; private set; }
    IActiveHandler  _activeHandler;

    
    public void Construct(IActiveHandler activeHandler)
    {
        _activeHandler = activeHandler;
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

      public async UniTask MoveCheckPoint(Transform transform)
      {
          _activeHandler.StopGame();
          await blackOutManager.Active();
          transform.position = CurrentCheckPoint.Transform.position;
          _activeHandler.ActiveGame();
      }
}
}