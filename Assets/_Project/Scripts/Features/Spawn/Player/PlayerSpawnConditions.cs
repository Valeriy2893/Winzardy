using _Project.Scripts.Core.World;
using _Project.Scripts.Features.Condition;
using _Project.Scripts.Features.Player.ECS;

namespace _Project.Scripts.Features.Spawn.Player
{
    public readonly struct PlayerSpawnConditions : IWorldCondition
    {
        private readonly LimitCondition<PlayerTag> _limit;

        public PlayerSpawnConditions(LimitCondition<PlayerTag> limit)
        {
            _limit = limit;
        }

        public bool IsMet(World world, float dt)
        {
            return _limit.IsMet();
        }
    }
}