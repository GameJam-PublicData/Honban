using System;
using UnityEngine;
using R3;

namespace StageSystem
{
public class BackgroundChanger : MonoBehaviour
{
    [Serializable]
    struct BackGroundData
    {
        public float SwitchYPos;
        public Material BackgroundMaterial;
    }
    [SerializeField] BackGroundData[] backGroundDataArray;
    [SerializeField] GameObject playerObj;
    
    SpriteRenderer _spriteRenderer;

    void Init()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (_spriteRenderer == null) throw new Exception("SpriteRendererが見つかりませんでした。");
        if (playerObj == null) throw new Exception("playerObjが設定されていません。");
        if (backGroundDataArray == null || backGroundDataArray.Length == 0) throw new Exception("backGroundDataArrayが設定されていません。");
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

    void ChangeBackground(int index) => _spriteRenderer.material = backGroundDataArray[index].BackgroundMaterial;
}
}
