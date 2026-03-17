using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{

public interface IMainMenuHolder
{
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
    public MainMenuManager(IMainMenuHolder mainMenuHolder)
    {
        _mainMenuHolder = mainMenuHolder;
        
    }

    public void Initialize()
    {
        _mainMenuHolder.GameEndButton.onClick.AddListener(OnGameEndButtonClicked);
        _mainMenuHolder.LicenseButton.onClick.AddListener(OnLicenseButtonClicked);
        _mainMenuHolder.AudioSettingButton.onClick.AddListener(OnAudioSettingButtonClicked);
    }
    
    void OnGameEndButtonClicked()
    {
        #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
        return;
        #endif
        Application.Quit();
    }
    
    void OnLicenseButtonClicked()
    {
        _mainMenuHolder.LicensePanel.SetActive(true);
    }

    void OnAudioSettingButtonClicked()
    {
        _mainMenuHolder.AudioSettingPanel.SetActive(true);
    }
}
}