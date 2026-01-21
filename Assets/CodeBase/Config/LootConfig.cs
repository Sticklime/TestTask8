using UnityEngine;

namespace CodeBase.Config
{
    [CreateAssetMenu(fileName = "LootConfig", menuName = "Config/LootConfig")]
    public class LootConfig : ScriptableObject
    {
        [field: SerializeField] public GameObject PrefabReference { get; private set; }
        [field: SerializeField] public float RadiusHitBox { get; private set; }
        [field: SerializeField] public int CountInOneDrop { get; private set; }
    }
}