using MainSystem.Audio;
using UnityEngine;
using VContainer;

namespace MainMenu
{
public class BGMPlayer : MonoBehaviour
{
    IAudioManager _audioManager;
    [SerializeField] string bgmName;

    [Inject]
    public void Construct(IAudioManager audioManager)
    {
        _audioManager = audioManager;
    }

    void Start()
    {
        _audioManager.PlayBGM(bgmName);
    }
}
}
