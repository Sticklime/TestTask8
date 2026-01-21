using UnityEngine;

namespace CodeBase.Config
{
    [CreateAssetMenu(fileName = "CharacterConfig", menuName = "Config/CharacterConfig")]
    public class CharacterConfig : ScriptableObject
    {
        [field: SerializeField] public GameObject PrefabReference { get; private set; }
        [field: SerializeField] public int MaxHealth { get; private set; }
        [field: SerializeField] public float MoveSpeed { get; private set; }
        [field: SerializeField] public int AttackCooldown { get; private set; }
        [field: SerializeField] public float RadiusCollider { get; private set; }
    }
}