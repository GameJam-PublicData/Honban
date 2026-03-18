using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
public class UIAnimator : MonoBehaviour
{
    [SerializeField] float animationFrameRate = 0.12f;
    [SerializeField] Sprite[] animationFrames; 
    Image _image;
    int _currentFrame = 0;
    
    void Start()
    {
        _image = GetComponent<Image>();
        
        StartCoroutine(UpdateAnimation());
    }

    IEnumerator UpdateAnimation()
    {
        yield return new WaitForSeconds(animationFrameRate);
        _image.sprite = animationFrames[_currentFrame];
        _currentFrame++;
        if(_currentFrame >= animationFrames.Length) _currentFrame = 0;
        StartCoroutine(UpdateAnimation());
    }
}
}
