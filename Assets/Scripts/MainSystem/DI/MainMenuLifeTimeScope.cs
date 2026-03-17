using Cysharp.Threading.Tasks;
using MainMenu;
using MainSystem.Audio;
using MainSystem.Scene;
using VContainer;
using VContainer.Unity;

namespace MainSystem.DI
{
public class MainMenuLifeTimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        // MainMenuSceneに特化した依存関係の登録をここに追加

        builder.RegisterComponentInHierarchy<IMainMenuHolder>();
        builder.Register<IMainMenuManager, MainMenuManager>(Lifetime.Scoped);
        builder.RegisterComponentInHierarchy<IAudioManager>();
    }
    
    void Start()
    {
        SceneInitialization().Forget();
    }

    async UniTask SceneInitialization()
    {
        var pub = Container.Resolve<ISceneInitializationPublisher>();
        //ここでMainMenuSceneの初期化処理を行う。例えば、UIのセットアップや、必要なデータのロードなど。
        Container.Resolve<IMainMenuManager>().Initialize();
        
        // 初期化が完了したら、ISceneInitializationPublisherを通じて、初期化が完了したことを通知します。
        pub.NotifyInitializationComplete();
    }
}
}