using System;
using StageSystem.Player;
using StageSystem.UI;
using UnityEngine;

namespace StageSystem.Clear
{
public class ClearChecker : MonoBehaviour
{
    [SerializeField] ClearUIManager  clearUIManager;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("クリア");
            clearUIManager.Initialize();
            other.GetComponent<PlayerJump>().enabled = false;
            other.GetComponent<PlayerController>().enabled = false;
            other.GetComponent<Rigidbody2D>().simulated = false;
            
        }
    }
}
}
  
  
