using _Project.Scripts.Core.World;
using _Project.Scripts.Features.Condition;
using _Project.Scripts.Features.EnemySpawner;

namespace _Project.Scripts.Features.Spawn.EnemySpawner
{
    public readonly struct EnemySpawnerConditions : IWorldCondition
    {
        private readonly LimitCondition<EnemySpawnerTag> _limit;

        public EnemySpawnerConditions(LimitCondition<EnemySpawnerTag> limit)
        {
            _limit = limit;
        }

        public bool IsMet(World world, float dt)
        {
            return _limit.IsMet();
        }
    }
}