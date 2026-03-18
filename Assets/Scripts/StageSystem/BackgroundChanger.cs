using System;
using UnityEngine;
using R3;
using DG.Tweening;

namespace StageSystem
{
public class BackgroundChanger : MonoBehaviour
{
    static readonly int ColorUp = Shader.PropertyToID("_Color_Up");
    static readonly int ColorDown = Shader.PropertyToID("_Color_Down");

    [Serializable]
    struct BackGroundData
    {
        public float SwitchYPos;
        public Color Up;
        public Color Down;
    }
    [SerializeField] BackGroundData[] backGroundDataArray;
    [SerializeField] GameObject playerObj;
    [SerializeField] float duration = 0.5f;

    Material _mat;

    void Init()
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (spriteRenderer == null) throw new Exception("SpriteRendererが見つかりませんでした。");
        if (playerObj == null) throw new Exception("playerObjが設定されていません。");
        if (backGroundDataArray == null || backGroundDataArray.Length == 0) throw new Exception("backGroundDataArrayが設定されていません。");
        
        _mat = spriteRenderer.material;
    }

    void Start()
    {
        try
        {
            Init();
            
            // Y座標の変化を監視して、高度に応じて背景を切り替える
            Observable.EveryUpdate()
                .Select(_ => ResolveIndex(playerObj.transform.position.y))
                .DistinctUntilChanged()
                .Subscribe(ChangeBackground)
                .AddTo(this);
        } catch (Exception e)
        {
            Debug.LogError($"BackgroundChanger: {e.Message}");
        }
    }
    
    // Y座標をインデックスに変換する
    int ResolveIndex(float playerY)
    {
        int result = 0;
        for (int i = 0; i < backGroundDataArray.Length; i++)
        {
            if (playerY >= backGroundDataArray[i].SwitchYPos)
                result = i;
        }
        return result;
    }

    void ChangeBackground(int index)
    {
        var data = backGroundDataArray[index];
    
        DOTween.To(() => _mat.GetColor(ColorUp), c => _mat.SetColor(ColorUp, c), data.Up, duration);
        DOTween.To(() => _mat.GetColor(ColorDown), c => _mat.SetColor(ColorDown, c), data.Down, duration);
    }
}
}
