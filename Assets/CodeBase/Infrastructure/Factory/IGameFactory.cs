using Unity.Entities;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    public interface IGameFactory
    {
        void CreateCharacter(Vector3 at);
        void CreateEnemy(Vector3 at);
        void CreateProjectile(Vector3 at, Vector3 targetDirection, EntityCommandBuffer commandBuffer);
        void CreateLoot(Vector3 at, EntityCommandBuffer commandBuffe);
    }
}