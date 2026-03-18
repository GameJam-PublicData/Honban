using System;
using StageSystem.Player;
using StageSystem.UI;
using UnityEngine;
using VContainer;

namespace StageSystem.Clear
{
public class ClearChecker : MonoBehaviour
{
    [Inject]
    public void Construct(IClearUIManager clearUIManager)
    {
        _clearUIManager = clearUIManager;
    }
    IClearUIManager _clearUIManager;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("クリア");
            _clearUIManager.Initialize(true, other.gameObject);
        }
    }
}
}
  
  
