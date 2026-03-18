using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace StageSystem.UI
{
public interface IInkModeUI
{
    void SwitchGravity(int index);
}
public class InkModeUI : MonoBehaviour, IInkModeUI
{
    [Header("Ink Mode")]
    [SerializeField] Image antigravity;
    [SerializeField] Image lowGravity;
    
    [Header("数値調節")]
    [SerializeField] Vector2 curTargetPos;
    [SerializeField] Vector2 nextTargetPos;

    [SerializeField] float height = 100f;
    [SerializeField] float duration = 0.5f;

    void Start()
    {
        if (antigravity == null) Debug.LogWarning("antigravity is null");
        if (lowGravity == null) Debug.LogWarning("lowGravity is null");
    }

    public void SwitchGravity(int index)
    {
        // 現在のモードアイコンを上から、次モードアイコンを下からカーブで移動させる
        switch (index)
        {
            case 0:
                // lowGravity -> antigravityへの切り替わり
                MoveCurveUI(antigravity, curTargetPos, true);
                MoveCurveUI(lowGravity, nextTargetPos, false);
                break;
            case 1:
                // antigravity -> lowGravityへの切り替わり
                MoveCurveUI(lowGravity, curTargetPos, true);
                MoveCurveUI(antigravity, nextTargetPos, false);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(index), index, null);
        }
    }
    
    void MoveCurveUI(Image target, Vector2 endPos, bool fromTop)
    {
        Vector2 targetPos = target.rectTransform.anchoredPosition;
        Vector2 startPos = targetPos;
        
        Vector2 controlPoint;
        
        // 始点と終点の中点を基準に、制御点を調節
        if (fromTop)
        {
            // 最下面に移動し、上から下への移動
            target.rectTransform.SetAsLastSibling();
            controlPoint = (startPos + endPos) / 2 + Vector2.down * height;
        }
        else
        {
            target.DOFade(0.6f, 0.25f);
            
            // 最上面に移動し、下から上への移動
            target.rectTransform.SetAsFirstSibling();
            controlPoint = (startPos + endPos) / 2 + Vector2.up * height;
        }
        
        DOTween.To(() => 0f, t =>
        {
            // 二次ベジェ曲線で位置を計算
            Vector2 pos = Mathf.Pow(1 - t, 2) * startPos + 2 * (1 - t) * t * controlPoint + Mathf.Pow(t, 2) * endPos;
            target.rectTransform.anchoredPosition = pos;
        }, 1f, duration).SetEase(Ease.OutCubic).onComplete = () => target.DOFade(1f, 0.25f);
    }
}
}