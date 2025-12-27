using _Project.Scripts.Core.ECS.Entity;
using _Project.Scripts.Core.World;
using _Project.Scripts.Features.Condition;

namespace _Project.Scripts.Features.Movement.Projectile
{
    public readonly struct ProjectileMovementConditions : IEntityCondition
    {
        private readonly DirectionCondition _direction;

        public ProjectileMovementConditions(DirectionCondition direction)
        {
            _direction = direction;
        }

        public bool IsMet(World world, float dt, EntityId entityId)
        {
            if (!_direction.IsMet(world, entityId))
                return false;

            return true;
        }
    }
}