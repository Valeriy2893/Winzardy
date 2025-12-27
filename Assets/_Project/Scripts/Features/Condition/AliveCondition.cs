using _Project.Scripts.Core.ECS.Components;
using _Project.Scripts.Core.ECS.Entity;
using _Project.Scripts.Core.World;

namespace _Project.Scripts.Features.Condition
{
    public readonly struct AliveCondition
    {
        public bool IsMet(World world, EntityId entity)
        {
            var healthPool = world.GetPool<Health>();
            
            if (!healthPool.Has(entity))
                return true;

            return healthPool.Get(entity).Current > 0;
        }
    }
}