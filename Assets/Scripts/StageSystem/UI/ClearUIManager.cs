using System;
using MainSystem.Scene;
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

    [SerializeField] Button MenuButton;
    ISceneLoader _sceneLoader;
    
    
    IActiveHandler _activeHandler;
    [Inject]
    public void Construct(IActiveHandler activeHandler,ISceneLoader  sceneLoader)
    {
        _activeHandler = activeHandler;
        _sceneLoader = sceneLoader;
    }

    void Awake()
    {
        MenuButton.onClick.AddListener(() =>
        {
            _sceneLoader.LoadScene(SceneType.MainMenuScene);
        });
    }

    void OnDestroy()
    {
        MenuButton.onClick.RemoveAllListeners();
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
            MenuButton.gameObject.SetActive(false);
        }
        else
        {
            clearImage.gameObject.SetActive(false);
            gameOverImage.gameObject.SetActive(true);
            MenuButton.gameObject.SetActive(true);
        }
        _activeHandler.StopGame();
    }
}
}
  
  
