using _Project.Scripts.Core.ECS.Entity;
using _Project.Scripts.Core.World;
using _Project.Scripts.Features.Condition;

namespace _Project.Scripts.Features.Player.ECS
{
    public readonly struct PlayerInputConditions : IEntityCondition
    {
        private readonly AliveCondition _alive;

        public PlayerInputConditions(AliveCondition alive)
        {
            _alive = alive;
        }

        public bool IsMet(World world, float dt, EntityId entityId)
        {
            if (!_alive.IsMet(world, entityId))
                return false;

            return true;
        }
    }
}