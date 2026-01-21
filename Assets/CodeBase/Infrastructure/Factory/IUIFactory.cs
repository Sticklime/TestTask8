using CodeBase.Infrastructure.Services.UI;
using CodeBase.Logic.UI.Restart;
using Unity.Entities;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    public interface IUIFactory
    {
        void Initialize(); 
        void CreateHUD(Entity characterEntity);
        void CreateEnemyHealthBar(Entity enemyEntity, GameObject parent);
        WindowBase CreateWindow(WindowType type);
    }
}