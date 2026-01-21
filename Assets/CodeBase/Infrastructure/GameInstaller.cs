using CodeBase.Infrastructure.Boot;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.Config;
using Unity.Entities;
using CodeBase.Infrastructure.Services.Input;
using CodeBase.Infrastructure.Services.ResourcesLoader;
using CodeBase.Infrastructure.Services.UI;
using CodeBase.Logic.UI.Coin;
using CodeBase.Logic.UI.Health;
using CodeBase.Logic.UI.Restart;
using MessagePipe;
using VContainer;
using VContainer.Unity;

namespace CodeBase.Infrastructure
{
    public class GameInstaller : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            RegisterEcsWorld(builder);
            RegisterGameState(builder);
            RegisterResourceLoad(builder);
            RegisterConfigProvider(builder);
            RegisterInput(builder);
            RegisterGameFactory(builder);
            RegisterUIFactory(builder);
            RegisterSystemBootstrapper(builder);
            RegisterOpenWindow(builder);
            RegisterMessagePipe(builder);
            RegisterEntryPoint(builder);
        }

        private static void RegisterUIFactory(IContainerBuilder builder) =>
            builder.Register<IUIFactory, UIFactory>(Lifetime.Singleton);

        private void RegisterResourceLoad(IContainerBuilder builder) =>
            builder.Register<IAssetLoader, AssetLoader>(Lifetime.Singleton);

        private void RegisterGameFactory(IContainerBuilder builder) =>
            builder.Register<IGameFactory, GameFactory>(Lifetime.Singleton);

        private void RegisterOpenWindow(IContainerBuilder builder) =>
            builder.Register<IOpenWindow, OpenWindow>(Lifetime.Singleton);

        private void RegisterInput(IContainerBuilder builder) =>
            builder.Register<IInputService, InputService>(Lifetime.Singleton);

        private void RegisterSystemBootstrapper(IContainerBuilder builder) =>
            builder.Register<SystemBootstrapper>(Lifetime.Singleton);

        private void RegisterGameState(IContainerBuilder builder) =>
            builder.Register<GameLoop>(Lifetime.Singleton);

        private void RegisterConfigProvider(IContainerBuilder builder) =>
            builder.Register<IConfigProvider, ConfigProvider>(Lifetime.Singleton);

        private void RegisterEcsWorld(IContainerBuilder builder)
        {
            var world = new World("GameWorld");
            builder.RegisterInstance(world)
                .As<World>()
                .AsSelf();
        }

        private void RegisterMessagePipe(IContainerBuilder builder)
        {
            MessagePipeOptions options = builder.RegisterMessagePipe();

            builder.RegisterMessageBroker<UpdateHealthView>(options);
            builder.RegisterMessageBroker<UpdateCoinView>(options);
            builder.RegisterMessageBroker<EndGame>(options);
            builder.RegisterMessageBroker<RestartGame>(options);

            builder.RegisterBuildCallback(resolver => { GlobalMessagePipe.SetProvider(resolver.AsServiceProvider()); });
        }


        private void RegisterEntryPoint(IContainerBuilder builder) =>
            builder.RegisterEntryPoint<EntryPoint>();
    }
}