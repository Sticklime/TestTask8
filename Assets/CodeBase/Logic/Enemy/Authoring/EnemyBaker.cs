using CodeBase.Config;
using CodeBase.Logic.Common.Move;
using CodeBase.Logic.Enemy;
using Unity.Entities;
using Unity.Transforms;

public class EnemyBaker : Baker<EnemyAuthoring>
{
    public override void Bake(EnemyAuthoring authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.Dynamic);
        
        EnemyConfig enemyConfig = authoring.EnemyConfig;
   
    }
}