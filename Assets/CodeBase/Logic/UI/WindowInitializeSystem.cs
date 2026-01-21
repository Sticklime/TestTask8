using CodeBase.Infrastructure.Services.UI;
using CodeBase.Logic.Character.Spawn;
using CodeBase.Logic.UI.Restart;
using MessagePipe;
using Unity.Entities;

namespace CodeBase.Logic.UI
{
    [DisableAutoCreation]
    [UpdateBefore(typeof(CharacterInitializeSystem))]
    public partial class WindowInitializeSystem : SystemBase
    {
        private readonly IOpenWindow _openWindow;
        private readonly ISubscriber<EndGame> _endGameSubscriber;

        public WindowInitializeSystem(IOpenWindow openWindow, ISubscriber<EndGame> endGameSubscriber)
        {
            _openWindow = openWindow;
            _endGameSubscriber = endGameSubscriber;
        }

        protected override void OnCreate()
        {
            _openWindow.RegisterWindow(WindowType.Restart);
            _openWindow.Close(WindowType.Restart);

            _endGameSubscriber.Subscribe(_ => OnEndGame());
        }

        private void OnEndGame() =>
            _openWindow.Open(WindowType.Restart);

        protected override void OnUpdate()
        {
        }
    }
}