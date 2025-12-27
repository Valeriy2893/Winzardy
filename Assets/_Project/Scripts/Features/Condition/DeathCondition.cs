using _Project.Scripts.Core.ECS.Components;
using _Project.Scripts.Core.ECS.Entity;
using _Project.Scripts.Core.World;

namespace _Project.Scripts.Features.Condition
{
    public readonly struct DeathCondition : IEntityCondition
    {
        public bool IsMet(World world, float dt, EntityId entity)
        {
            ref var hp = ref world.GetPool<Health>().Get(entity);
            return hp.Current <= 0;
        }
    }
}