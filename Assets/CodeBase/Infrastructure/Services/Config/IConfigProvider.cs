using CodeBase.Config;

namespace CodeBase.Infrastructure.Services.Config
{
    public interface IConfigProvider
    {
        void Initialize();
        CharacterConfig GetCharacterConfig();
        CameraConfig GetCameraConfig();
        EnemyConfig GetEnemyConfig();
        ProjectileConfig GetProjectileConfig();
        LootConfig GetLootConfig();
    }
}