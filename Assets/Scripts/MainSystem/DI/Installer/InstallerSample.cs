using System;
using VContainer;
using VContainer.Unity;

namespace MainSystem.DI.Installer
{
//Installerは、DIコンテナに依存関係を登録するためのインターフェースです。これを実装することで、特定のモジュールや機能に関連する依存関係をまとめて管理できます。
//LifeTimeScopeで全てを登録すると、コードが煩雑になる可能性があるため、Installerを使用して依存関係を整理することが推奨されます。
//Installerの呼び出し方法は、LifeTimeScopeのConfigureメソッド内でEnqueueメソッドを使用して行います。これにより、Installer内で定義された依存関係がDIコンテナに登録されます。
// Enqueue(new InstallerSample());
public class InstallerSample : IInstaller
{
    public void Install(IContainerBuilder builder)
    {
        // ここで依存関係を登録します。例えば:
        // builder.Register<Interface, Implementation>(Lifetime.Scoped);
    }
}
}