using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MainSystem.Scene
{
public interface ISceneLoader
{
    public UniTask LoadScene(SceneType sceneType);
}

public enum SceneType
{
    BootScene,
    MainMenuScene,
    StageScene,// またはStageNScene
    // 追加のシーンタイプをここに定義
}

public class SceneLoader : ISceneLoader
{
    ISceneInitializationAwaiter _awaiter;
    public SceneLoader(ISceneInitializationAwaiter awaiter)
    {
        _awaiter = awaiter;
    }
    
    public async UniTask LoadScene(SceneType sceneType)
    {
        var asyncOperation = SceneManager.LoadSceneAsync(sceneType.ToString());
        
        if(asyncOperation == null)
        {
            Debug.LogError($"Scene {sceneType} could not be loaded.");
            return;
        }
        _awaiter.Reset();
        await _awaiter.WaitForInitialization();
        
        // シーンの初期化が完了するまで待機
        // ロード先のシーンは、初期化が完了したら
        // ISceneInitializationPublisher.NotifyInitializationComplete()を呼び出す必要があります
        
       
    }
}
}