using Unity.Entities;
using UnityEngine.AI;

namespace CodeBase.Logic.Enemy.Follow
{
    public class NavMeshComponent : IComponentData
    {
        public NavMeshAgent Agent;
    }
}