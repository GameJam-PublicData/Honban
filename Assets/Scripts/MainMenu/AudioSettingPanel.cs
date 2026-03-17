using MainSystem.Audio;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace MainMenu
{
public class AudioSettingPanel : MonoBehaviour
{
    IAudioManager _audioManager;
    [Inject]
    public void Construct(IAudioManager audioManager)
    {
        Debug.LogError("AudioSettingPanelにAudioManagerが注入されました");
        _audioManager = audioManager;
    }
    
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider seSlider;
    [SerializeField] Button closeButton;
    
    void Start()
    {
        masterSlider.value = (_audioManager.GetVolume(AudioCategory.Master)+80) /100f;
        bgmSlider.value = (_audioManager.GetVolume(AudioCategory.BGM)+80) / 100f;
        seSlider.value = (_audioManager.GetVolume(AudioCategory.SE)+80) / 100f;
        
        masterSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
        bgmSlider.onValueChanged.AddListener(OnBGMVolumeChanged);
        seSlider.onValueChanged.AddListener(OnSEVolumeChanged);
        
        closeButton.onClick.AddListener(() => gameObject.SetActive(false));
    }
    
    void OnMasterVolumeChanged(float value)
    {
        _audioManager.SetVolume(AudioCategory.Master, value);
    }
    
    void OnBGMVolumeChanged(float value)
    {
        _audioManager.SetVolume(AudioCategory.BGM, value);
    }
    
    void OnSEVolumeChanged(float value)
    {
        Debug.Log($"SEの音量が{value }に変更されました");
        _audioManager.SetVolume(AudioCategory.SE,value);
    }
}
}
  
  
