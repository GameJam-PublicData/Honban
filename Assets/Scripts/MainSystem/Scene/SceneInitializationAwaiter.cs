using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MainSystem.Scene
{
public interface ISceneInitializationPublisher
{
    void NotifyInitializationComplete();
    void NotifyInitializationFailed(Exception ex);
}
public interface ISceneInitializationAwaiter
{
    UniTask WaitForInitialization();
    void Reset();
}

public class SceneInitializationAwaiter : ISceneInitializationAwaiter, ISceneInitializationPublisher
{
    UniTaskCompletionSource _tcs;
    
    public async UniTask WaitForInitialization()
    {
        _tcs = new UniTaskCompletionSource();

        try
        {
            await _tcs.Task;
        }
        catch (Exception e)
        {
            Debug.LogError($"シーンの初期化が失敗しました: {e.Message}");
            throw;
        }
    }
    
    public void NotifyInitializationComplete()
    {
        Debug.Log("シーンの初期化が完了しました");
        _tcs?.TrySetResult();
    }
    
    public void NotifyInitializationFailed(Exception ex)
    {
        _tcs.TrySetException(ex);
    }
    
    public void Reset()
    {
        _tcs = new UniTaskCompletionSource();
    }
}
}