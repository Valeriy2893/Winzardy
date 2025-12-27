using _Project.Scripts.Core.ECS;
using _Project.Scripts.Core.ECS.Components;
using _Project.Scripts.Core.World;
using _Project.Scripts.Features.Condition;
using _Project.Scripts.Features.Player.ECS;

namespace _Project.Scripts.Features.Spawn.Projectile
{
    public readonly struct PlayerProjectileSpawnConditions : IWorldCondition
    {
        private readonly AliveCondition _alive;
        private readonly TimerCondition _timer;
        private readonly EcsFilter<PlayerTag> _filter;

        public PlayerProjectileSpawnConditions(AliveCondition alive, TimerCondition timer, EcsFilter<PlayerTag> filter)
        {
            _alive = alive;
            _timer = timer;
            _filter = filter;
        }

        public bool IsMet(World world, float dt)
        {
            if (_filter.Entities.Count == 0) return false;
            if (!_alive.IsMet(world, _filter.Entities[0])) return false;
            if (!world.GetPool<Timer>().Has(_filter.Entities[0])) return false;
            ref var timer = ref world.GetPool<Timer>().Get(_filter.Entities[0]);
            if (!_timer.IsMet(dt, ref timer)) return false;
            return true;
        }
    }
}