using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using StageSystem.Player;
using UnityEngine;
using VContainer;

namespace StageSystem.CheckPoint
{
public interface ICheckPointManager
{
    public ICheckPoint CurrentCheckPoint { get; }
    public void ThroughCheckPoint(ICheckPoint checkPoint);
    public UniTask MoveCheckPoint();
    
}

public class CheckPointManager : MonoBehaviour, ICheckPointManager
{
    List<ICheckPoint> _checkPoints = new();
    [SerializeField] CheckPointBlackOutManager blackOutManager;

    public ICheckPoint CurrentCheckPoint { get; private set; }
    IActiveHandler  _activeHandler;

    Transform _playerTransform;
    
    [Inject]
    public void Construct(IActiveHandler activeHandler,[Key("Player")] GameObject playerTransform)
    {
        _activeHandler = activeHandler;
        _playerTransform = playerTransform.transform;
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
          int currentIndex = _checkPoints.IndexOf(CurrentCheckPoint);
          int newIndex = _checkPoints.IndexOf(checkPoint); 
          if (newIndex > currentIndex) CurrentCheckPoint = checkPoint;//新しいチェックポイントが現在のチェックポイントよりも後にある場合、更新する
      }

      public async UniTask MoveCheckPoint()
      {
          _activeHandler.StopGame();
          await blackOutManager.Active();
          _playerTransform.position = CurrentCheckPoint.Transform.position;
          _activeHandler.ActiveGame();
      }
}
}