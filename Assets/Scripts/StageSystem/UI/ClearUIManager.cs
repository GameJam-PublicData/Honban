using System;
using UnityEngine;
using UnityEngine.UI;

namespace StageSystem.UI
{
public interface IClearUIManager
{
    public void Initialize(bool isClear, GameObject playerObject);
}
public class ClearUIManager : MonoBehaviour , IClearUIManager
{
    [SerializeField] Image clearImage;
    [SerializeField] Image gameOverImage;
    
    
    public void Initialize(bool isClear, GameObject playerObject)
    {
        Debug.Log("クリアUI表示");
        gameObject.SetActive(true);
        if(isClear)
        {
            clearImage.gameObject.SetActive(true);
            gameOverImage.gameObject.SetActive(false);
        }
        else
        {
            clearImage.gameObject.SetActive(false);
            gameOverImage.gameObject.SetActive(true);
        }
    }
}
}
  
  
