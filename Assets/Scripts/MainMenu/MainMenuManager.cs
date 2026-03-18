using MainSystem.Audio;
using MainSystem.Scene;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{

public interface IMainMenuHolder
{
    Button GameStartButton { get; }
    Button GameEndButton { get; }
    Button LicenseButton { get; }
    GameObject LicensePanel { get; }
    Button AudioSettingButton { get; }
    GameObject AudioSettingPanel { get; }
}

public interface IMainMenuManager
{
    void Initialize();
}
public class MainMenuManager : IMainMenuManager
{
    IMainMenuHolder _mainMenuHolder;
    IAudioManager _audioManager;
    ISceneLoader _sceneLoader;
    public MainMenuManager(IMainMenuHolder mainMenuHolder,IAudioManager audioManager,ISceneLoader sceneLoader)
    {
        _mainMenuHolder = mainMenuHolder;
        _audioManager = audioManager;
        _sceneLoader = sceneLoader;
    }

    public void Initialize()
    {
        _mainMenuHolder.GameStartButton.onClick.AddListener(OnGameStartButtonClicked);
        _mainMenuHolder.GameEndButton.onClick.AddListener(OnGameEndButtonClicked);
        _mainMenuHolder.LicenseButton.onClick.AddListener(OnLicenseButtonClicked);
        _mainMenuHolder.AudioSettingButton.onClick.AddListener(OnAudioSettingButtonClicked);
    }


    void OnGameStartButtonClicked()
    {
        _audioManager.PlaySE("ButtonPushSound");
        _sceneLoader.LoadScene(SceneType.GameScene);
    }
    
    void OnGameEndButtonClicked()
    {
        _audioManager.PlaySE("ButtonPushSound");
        #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
        return;
        #endif
        Application.Quit();
    }
    
    void OnLicenseButtonClicked()
    {
        _audioManager.PlaySE("ButtonPushSound");
        _mainMenuHolder.LicensePanel.SetActive(true);
    }

    void OnAudioSettingButtonClicked()
    {
        _audioManager.PlaySE("ButtonPushSound");
        _mainMenuHolder.AudioSettingPanel.SetActive(true);
    }
}
}