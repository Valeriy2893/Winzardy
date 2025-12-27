using _Project.Scripts.Core.ECS.Components;
using _Project.Scripts.Core.ECS.Entity;
using _Project.Scripts.Core.World;

namespace _Project.Scripts.Features.Condition
{
    public readonly struct LifetimeExpiredCondition : IEntityCondition
    {
        public bool IsMet(World world, float dt, EntityId entity)
        {
            ref var lifetime = ref world.GetPool<Lifetime>().Get(entity);

            lifetime.TimeLeft -= dt;
            return lifetime.TimeLeft <= 0f;
        }
    }
}