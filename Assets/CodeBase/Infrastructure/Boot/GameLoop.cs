using System;
using CodeBase.Logic.UI.Restart;
using MessagePipe;

namespace CodeBase.Infrastructure.Boot
{
    public class GameLoop : IDisposable
    {
        private readonly SystemBootstrapper _systemBootstrapper;
        private readonly ISubscriber<EndGame> _endSubscriber;
        private readonly ISubscriber<RestartGame> _restartSubscriber;

        private IDisposable _endSubscription;
        private IDisposable _restartSubscription;

        public GameLoop(SystemBootstrapper systemBootstrapper, ISubscriber<EndGame> endSubscriber, ISubscriber<RestartGame> restartSubscriber)
        {
            _systemBootstrapper = systemBootstrapper;
            _endSubscriber = endSubscriber;
            _restartSubscriber = restartSubscriber;
        }

        public void StartGame()
        {
            _systemBootstrapper.Initialize();

            _endSubscription = _endSubscriber.Subscribe(_ => StopGame());
            _restartSubscription = _restartSubscriber.Subscribe(_ => RestartGame());
        }

        private void StopGame() => 
            _systemBootstrapper.StopGame();

        private void RestartGame() => 
            _systemBootstrapper.RestartGame();

        public void Dispose()
        {
            _endSubscription?.Dispose();
            _restartSubscription?.Dispose();
        }
    }
}