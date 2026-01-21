using UnityEngine;

namespace CodeBase.Config
{
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "Config/EnemyConfig")]
    public class EnemyConfig : ScriptableObject
    {
        [field: SerializeField] public GameObject PrefabReference { get; private set; }
        [field: SerializeField] public float AttackRange { get; private set; }
        [field: SerializeField] public float MoveSpeed { get; private set; }
        [field: SerializeField] public float SpawnInterval { get; private set; }
        [field: SerializeField] public float SpawnOffset { get; private set; }
        [field: SerializeField] public int MaxHealth { get; private set; }
        [field: SerializeField] public int DamageValue { get; private set; }
        [field: SerializeField] public float AttackCooldown { get; private set; }
        [field: SerializeField] public float RadiusHitBox { get; private set; }
        [field: SerializeField, Range(0, 100)] public float ChanceFallingOutLoot { get; private set; }
    }
}