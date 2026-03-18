using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace StageSystem.CheckPoint
{
public class CheckPointBlackOutManager : MonoBehaviour
{
    [SerializeField] Image blackOut;
    [SerializeField] float fadeInTime = 1f;
    [SerializeField] float fadeOutTime = 0.3f;
    
    
    public async UniTask Active()
    {
        blackOut.DOFade(1, fadeInTime).SetEase(Ease.Linear);
        await UniTask.Delay((int)(fadeInTime * 1000));
        DeActive().Forget();
    }
    
    async UniTask DeActive()
    {
        blackOut.DOFade(0, fadeOutTime).SetEase(Ease.Linear);
        await UniTask.Delay((int)(fadeOutTime * 1000));
    }
}
}
  
  
