using CodeBase.Config;
using CodeBase.Infrastructure.Services.ResourcesLoader;

namespace CodeBase.Infrastructure.Services.Config
{
    /// <summary>
    /// Получение конфигов сделано через методы т.к. в продакшене чаще всего есть разные типы объекта
    /// </summary>
    public class ConfigProvider : IConfigProvider
    {
        private readonly IAssetLoader _assetLoader;
        private CharacterConfig _characterConfig;
        private CameraConfig _cameraConfig;
        private EnemyConfig _enemyConfig;
        private ProjectileConfig _projectileConfig;
        private LootConfig _lootConfig;

        public ConfigProvider(IAssetLoader assetLoader)
        {
            _assetLoader = assetLoader;
        }

        public void Initialize()
        {
            _characterConfig = _assetLoader.Load<CharacterConfig>(AssetPath.CHARACTER_CONFIG_PATH);
            _cameraConfig = _assetLoader.Load<CameraConfig>(AssetPath.CAMERA_CONFIG_PATH);
            _enemyConfig = _assetLoader.Load<EnemyConfig>(AssetPath.ENEMY_CONFIG_PATH);
            _projectileConfig = _assetLoader.Load<ProjectileConfig>(AssetPath.PROJECTILE_CONFIG_PATH);
            _lootConfig = _assetLoader.Load<LootConfig>(AssetPath.LOOT_CONFIG_PATH);
        }

        public CharacterConfig GetCharacterConfig() =>
            _characterConfig;

        public CameraConfig GetCameraConfig() =>
            _cameraConfig;

        public EnemyConfig GetEnemyConfig() =>
            _enemyConfig;

        public ProjectileConfig GetProjectileConfig() =>
            _projectileConfig;

        public LootConfig GetLootConfig() =>
            _lootConfig;
    }
}