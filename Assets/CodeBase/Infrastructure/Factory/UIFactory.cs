using System.Collections.Generic;
using System.Linq;
using CodeBase.Infrastructure.Services.ResourcesLoader;
using CodeBase.Infrastructure.Services.UI;
using CodeBase.Logic.Common.Move;
using Unity.Entities;
using CodeBase.Logic.UI;
using CodeBase.Logic.UI.Coin;
using CodeBase.Logic.UI.Health;
using CodeBase.Logic.UI.Restart;
using MessagePipe;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace CodeBase.Infrastructure.Factory
{
    public class UIFactory : IUIFactory
    {
        private readonly IAssetLoader _assetLoader;
        private readonly World _world;
        private readonly IObjectResolver _objectResolver;
        private readonly ISubscriber<UpdateHealthView> _updateHealthEvent;
        private readonly ISubscriber<UpdateCoinView> _updateCoinEvent;
        private readonly IPublisher<RestartGame> _restartPublisher;

        private GameObject _hudPrefab;
        private GameObject _enemyHealthBarPrefab;
        private List<WindowBase> _windowPrefabs;

        public UIFactory(IAssetLoader assetLoader, World world, IObjectResolver objectResolver,
            ISubscriber<UpdateHealthView> updateHealthEvent,
            ISubscriber<UpdateCoinView> updateCoinEvent, IPublisher<RestartGame> restartPublisher)
        {
            _assetLoader = assetLoader;
            _world = world;
            _objectResolver = objectResolver;
            _updateHealthEvent = updateHealthEvent;
            _updateCoinEvent = updateCoinEvent;
            _restartPublisher = restartPublisher;
        }

        public void Initialize()
        {
            _hudPrefab = _assetLoader.Load<GameObject>(AssetPath.HUD_PREFAB_PATH);
            _enemyHealthBarPrefab = _assetLoader.Load<GameObject>(AssetPath.ENEMY_HEALTH_PREFAB_PATH);
            _windowPrefabs = _assetLoader.LoadAll<WindowBase>(AssetPath.RESTART_PREFAB_PATH).ToList();
        }

        public void CreateHUD(Entity characterEntity)
        {
            GameObject hudInstance = Object.Instantiate(_hudPrefab);

            EntityManager entityManager = _world.EntityManager;
            Entity hudEntity = entityManager.CreateEntity();

            HealthView healthView = hudInstance.GetComponent<HealthView>();
            CoinView coinView = hudInstance.GetComponent<CoinView>();

            HealthPresenter healthPresenter = new HealthPresenter(healthView, characterEntity, _updateHealthEvent);
            CoinPresenter coinPresenter = new CoinPresenter(coinView, _updateCoinEvent);

            entityManager.AddComponentData(hudEntity, new TransformReference { Transform = hudInstance.transform });

            healthPresenter.Enable();
            coinPresenter.Enable();
        }

        public void CreateEnemyHealthBar(Entity enemyEntity, GameObject parent)
        {
            GameObject enemyHealthInstance = Object.Instantiate(_enemyHealthBarPrefab, parent.transform, false);
            HealthView healthView = enemyHealthInstance.GetComponent<HealthView>();
            HealthPresenter healthPresenter = new HealthPresenter(healthView, enemyEntity, _updateHealthEvent);
            healthPresenter.Enable();
        }

        public WindowBase CreateWindow(WindowType type)
        {
            var window = _windowPrefabs.FirstOrDefault(w => w.WindowType == type);

            WindowBase restartInstance = _objectResolver.Instantiate(window);

            return restartInstance;
        }
    }
}