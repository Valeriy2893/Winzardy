using _Project.Scripts.Core.ECS;
using _Project.Scripts.Core.ECS.Components;
using _Project.Scripts.Core.World;
using _Project.Scripts.Features.Condition;
using _Project.Scripts.Features.EnemySpawner;

namespace _Project.Scripts.Features.Spawn.Enemy
{
    public readonly struct EnemySpawnConditions : IWorldCondition
    {
        private readonly ViewportCondition _viewport;
        private readonly EcsFilter<EnemySpawnerTag> _filter;
        private readonly TimerCondition _timer;

        public EnemySpawnConditions(EcsFilter<EnemySpawnerTag> filter, TimerCondition timer, ViewportCondition viewport)
        {
            _filter = filter;
            _timer = timer;
            _viewport = viewport;
        }

        public bool IsMet(World world, float dt)
        {
            if (_filter.Entities.Count == 0) return false;
            if (!_viewport.IsMet()) return false;
            
            var entities = _filter.Entities;

            for (int i = 0; i < entities.Count; i++)
            {
                ref var timer = ref world.GetPool<Timer>().Get(entities[i]);

                if (_timer.IsMet(dt, ref timer))
                    return true;
            }

            return false;
        }
    }
}