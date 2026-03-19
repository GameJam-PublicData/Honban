using MainSystem.CoreFlow;
using MainSystem.Save;
using MainSystem.Scene;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace MainSystem.DI
{
public class RootLifeTimeScope : LifetimeScope
{
    [SerializeField] bool notLoadBootScene = true;
    protected override void Configure(IContainerBuilder builder)
    {
        // builder.Register<interface,class>();
        builder.Register<BootManager>(Lifetime.Singleton);
        builder.Register<ISceneLoader,SceneLoader>(Lifetime.Singleton);
        builder.Register<SceneInitializationAwaiter>(Lifetime.Singleton).AsImplementedInterfaces();
        builder.Register<ISaveDataManager,SaveDataManager>(Lifetime.Singleton);
    }

    void Start()
    {
        var bootManager = Container.Resolve<BootManager>();
        bootManager.Initialize(notLoadBootScene);
    }
}
}