using UnityEngine;

namespace CodeBase.Config
{
    [CreateAssetMenu(menuName = "Config/ProjectileConfig", fileName = "ProjectileConfig")]
    public class ProjectileConfig : ScriptableObject
    {
        [field: SerializeField] public GameObject Prefab { get; private set; }
        [field: SerializeField] public float MoveSpeed { get; private set; }
        [field: SerializeField] public float LifeTime { get; private set; }
        [field: SerializeField] public int Damage { get; private set; }
    }
}