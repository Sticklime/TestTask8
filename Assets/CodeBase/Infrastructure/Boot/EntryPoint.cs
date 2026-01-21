using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.Config;
using CodeBase.Infrastructure.Services.Input;
using VContainer.Unity;

namespace CodeBase.Infrastructure.Boot
{
    public class EntryPoint : IStartable
    {
        private readonly IInputService _inputService;
        private readonly IConfigProvider _configProvider;
        private readonly IUIFactory _uiFactory;
        private readonly GameLoop _gameLoop;

        public EntryPoint(IInputService inputService, IConfigProvider configProvider, IUIFactory uiFactory,
            GameLoop gameLoop)
        {
            _inputService = inputService;
            _configProvider = configProvider;
            _uiFactory = uiFactory;
            _gameLoop = gameLoop;
        }

        public void Start()
        {
            _configProvider.Initialize();
            _uiFactory.Initialize();
            _inputService.Initialize();

            _gameLoop.StartGame();
        }
    }
}