using System;
using StageSystem.Player;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace StageSystem.UI
{
public interface IClearUIManager
{
    public void Initialize(bool isClear);
}
public class ClearUIManager : MonoBehaviour , IClearUIManager
{
    [SerializeField] Image clearImage;
    [SerializeField] Image clearBackground;
    [SerializeField] Image gameOverImage;

    IActiveHandler _activeHandler;
    [Inject]
    public void Construct(IActiveHandler activeHandler)
    {
        _activeHandler = activeHandler;
    }
    
    public void Initialize(bool isClear)
    {
        Debug.Log("クリアUI表示");
        gameObject.SetActive(true);
        if(isClear)
        {
            clearImage.gameObject.SetActive(true);
            clearBackground.gameObject.SetActive(true);
            gameOverImage.gameObject.SetActive(false);
        }
        else
        {
            clearImage.gameObject.SetActive(false);
            gameOverImage.gameObject.SetActive(true);
        }
        _activeHandler.StopGame();
    }
}
}
  
  
