using System;
using Cysharp.Threading.Tasks;
using MainSystem.Audio;
using MainSystem.DI.Installer;
using MainSystem.Scene;
using StageSystem;
using StageSystem.Ink;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace MainSystem.DI
{
public class StageLifeTimeScope : LifetimeScope
{
    [SerializeField] GameObject inkEffectPrefab;
    protected override void Configure(IContainerBuilder builder)
    {
        // StageSceneに特化した依存関係の登録をここに追加
        builder.RegisterComponentInHierarchy<IAudioManager>();
        builder.RegisterComponentInHierarchy<InkSelectManager>().As<ICurrentInkEffect>();
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
