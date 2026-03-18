using System;
using UnityEngine;

namespace StageSystem.CheckPoint
{
public interface ICheckPoint
{
    public void Initialize(ICheckPointManager checkPointManager);
    Transform Transform { get; }
}
public class CheckPoint :MonoBehaviour,ICheckPoint
{
    ICheckPointManager _checkPointManager;
    public Transform Transform => transform;
    void OnTriggerEnter2D(Collider2D other)
    { 
        if (other.CompareTag("Player"))
        {
            _checkPointManager.ThroughCheckPoint(this);
        }
        
    }

    public void Initialize(ICheckPointManager checkPointManager)
    {
        Debug.Log($"Initializing checkpoint: {transform.name}");
        _checkPointManager = checkPointManager;
    }
}
}