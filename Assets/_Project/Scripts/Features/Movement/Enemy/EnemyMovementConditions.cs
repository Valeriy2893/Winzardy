using _Project.Scripts.Core.ECS.Entity;
using _Project.Scripts.Core.World;
using _Project.Scripts.Features.Condition;

namespace _Project.Scripts.Features.Movement.Enemy
{
    public readonly struct EnemyMovementConditions : IEntityCondition
    {
        private readonly AliveCondition _alive;
        private readonly DirectionCondition _direction;

        public EnemyMovementConditions(AliveCondition alive, DirectionCondition direction)
        {
            _alive = alive;
            _direction = direction;
        }

        public bool IsMet(World world, float dt, EntityId entityId)
        {
            if (!_alive.IsMet(world, entityId))
                return false;

            if (!_direction.IsMet(world, entityId))
                return false;

            return true;
        }
    }
}