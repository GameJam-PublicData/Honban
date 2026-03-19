using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
public class HowToPlayButton : MonoBehaviour
{
    [SerializeField] Button playButton;
    [SerializeField] Button quitButton;
    
    [SerializeField] RectTransform image;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playButton.onClick.AddListener(OnPlayButtonPressed);
        quitButton.onClick.AddListener(OnQuitButtonPressed);

        image.localScale = new Vector3(1, 0, 1);
    }

    void OnPlayButtonPressed()
    {
        image.DOScale(new Vector3(1, 1, 1), 0.5f).SetEase(Ease.OutBack);
    }

    void OnQuitButtonPressed()
    {
        image.DOScale(new Vector3(1, 0, 1), 0.5f).SetEase(Ease.InBack);
    }
}
}
