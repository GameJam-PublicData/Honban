using Cysharp.Threading.Tasks;
using MainSystem.Audio;
using MainSystem.Scene;
using StageSystem.Area;
using StageSystem.CheckPoint;
using StageSystem.Ink;
using StageSystem.Player;
using StageSystem.UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace MainSystem.DI
{
public class StageLifeTimeScope : LifetimeScope
{
    [SerializeField] GameObject playerTransform;
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterInstance(playerTransform).AsSelf().Keyed("Player");
        // StageSceneに特化した依存関係の登録をここに追加
        builder.RegisterComponentInHierarchy<IAudioManager>();
        builder.RegisterComponentInHierarchy<InkSelectManager>().As<ICurrentInkEffect>().AsSelf();
        builder.RegisterComponentInHierarchy<InkManager>().As<IInkManager>();
        builder.Register<IStrokeBuilder,StrokeBuilder>(Lifetime.Scoped);
        builder.RegisterComponentInHierarchy<InkAmount>().As<IInkAmount>();
        builder.RegisterComponentInHierarchy<ICursorTrail>();
        builder.RegisterEntryPoint<AreaController>(Lifetime.Scoped).As<IAreaController>();
        builder.RegisterComponentInHierarchy<ICheckPointManager>();
        builder.RegisterComponentInHierarchy<IClearUIManager>();
        builder.RegisterComponentInHierarchy<IActiveHandler>();
    }

    void Start()
    {
        SceneInitialization().Forget();
    }
    
    
    // StageSceneの初期化処理をここに追加
    async UniTask SceneInitialization()
    {
        var pub = Container.Resolve<ISceneInitializationPublisher>();
        //ここでStageSceneの初期化処理を行う。例えば、UIのセットアップや、必要なデータのロードなど。
        
        await UniTask.Yield();//ダミー
        
        // 初期化が完了したら、ISceneInitializationPublisherを通じて、初期化が完了したことを通知します。
        pub.NotifyInitializationComplete();
    }
}
}
