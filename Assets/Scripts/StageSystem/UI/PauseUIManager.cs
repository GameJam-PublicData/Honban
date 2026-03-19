using System;
using Cysharp.Threading.Tasks;
using InputSystemActions;
using MainSystem.Scene;
using StageSystem.CheckPoint;
using StageSystem.Player;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace StageSystem.UI
{
public class PauseUIManager : MonoBehaviour
{
    [SerializeField] Canvas pauseCanvas;
    [SerializeField] Button menuButton;
    [SerializeField] Button backButton;
    [SerializeField] Button bakcCheckPointButton;

    IActiveHandler  _activeHandler;
    InputActions _inputActions;
    ISceneLoader  _sceneLoader;
    ICheckPointManager _checkPointManager;
    
    [Inject]
    public void Construct(
        IActiveHandler  activeHandler,
        ISceneLoader  sceneLoader,
        ICheckPointManager checkPointManager)
    {
        _activeHandler = activeHandler;
        _sceneLoader = sceneLoader;
        _checkPointManager = checkPointManager;
    }

    void Start()
    {
        menuButton.onClick.AddListener(LoadMenuScene);
        backButton.onClick.AddListener(Resume);
        bakcCheckPointButton.onClick.AddListener(BackCheckPoint);
    }

    void OnDestroy()
    {
        menuButton.onClick.RemoveListener(LoadMenuScene);
        backButton.onClick.RemoveListener(Resume);
        bakcCheckPointButton.onClick.RemoveListener(BackCheckPoint);
    }

    void OnEnable()
    {
        _inputActions = new InputActions();
        _inputActions.Player.Enable();
        _inputActions.Player.Escape.started += OnEscapeButton;
    }

    void OnDisable()
    {
        _inputActions.Player.Escape.started -= OnEscapeButton;
        _inputActions.Player.Disable();
    }


    void OnEscapeButton(InputAction.CallbackContext ctx)
    {
        if (pauseCanvas.gameObject.activeInHierarchy) Resume();
        else Pause();
    }

    void LoadMenuScene()
    {
        _sceneLoader.LoadScene(SceneType.MainMenuScene).Forget();
    }
    
    public void Pause()
    {
        pauseCanvas.gameObject.SetActive(true);
        _activeHandler.StopGame();
    }

    public void Resume()
    {
        pauseCanvas.gameObject.SetActive(false);
        _activeHandler.ActiveGame();
    }

    public void BackCheckPoint()
    {
        _checkPointManager.MoveCheckPoint();
        pauseCanvas.gameObject.SetActive(false);
    }
    public void PostStart()
    {
        OnEnable();
    }
}
}
  
  
