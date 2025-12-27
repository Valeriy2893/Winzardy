using _Project.Scripts.Core.ECS.Components;
using _Project.Scripts.Core.ECS.Entity;
using _Project.Scripts.Core.World;

namespace _Project.Scripts.Features.Condition
{
    public readonly struct DirectionCondition
    {
        public bool IsMet(World world, EntityId entityId)
        {
            return world.GetPool<Direction>().Has(entityId);
        }
    }
}