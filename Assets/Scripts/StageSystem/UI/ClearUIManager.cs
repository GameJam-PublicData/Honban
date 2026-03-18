using System;
using UnityEngine;

namespace StageSystem.UI
{
public class ClearUIManager : MonoBehaviour
{
    void Awake()
    {
        //gameObject.SetActive(false);
    }

    public void Initialize()
    {
        Debug.Log("クリアUI表示");
        gameObject.SetActive(true);
    }
}
}
  
  
