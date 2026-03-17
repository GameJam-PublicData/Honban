using Cysharp.Threading.Tasks;
using MainSystem.Scene;

namespace MainSystem.CoreFlow 
{
public class BootManager
{
    ISceneLoader _sceneLoader;
    public BootManager(ISceneLoader sceneLoader)
    {
        _sceneLoader = sceneLoader;
    }
    public void Initialize(bool isDebug)
    {
        // ゲームに必要な初期化処理

        #if UNITY_EDITOR
        if (isDebug) return;
        #endif
        
        _sceneLoader.LoadScene(SceneType.MainMenuScene).Forget();
    }
    
}
}