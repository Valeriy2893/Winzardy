using _Project.Scripts.Core.ECS.Components;
using _Project.Scripts.Core.World;
using _Project.Scripts.Features.Coin;
using _Project.Scripts.Features.Enemy;
using _Project.Scripts.Infrastructure.EventBus;
using _Project.Scripts.Infrastructure.EventBus.Events;
using Random = System.Random;

namespace _Project.Scripts.Features.Spawn.Coin
{
    public sealed class CoinDropListener : IEventListener<EntityDestroyedEvent>
    {
        private readonly World _world;
        private readonly EnemyConfig _enemyConfig;
        private readonly Random _random;

        public CoinDropListener(World world, EnemyConfig enemyConfig, Random random)
        {
            _world = world;
            _enemyConfig = enemyConfig;
            _random = random;
        }

        public void OnEvent(in EntityDestroyedEvent e)
        {
            var enemy = e.Entity;
            if (!_world.GetPool<EnemyTag>().Has(enemy)) return;
            if (!_world.GetPool<Position>().Has(enemy)) return;
            if (_random.NextDouble() > _enemyConfig.CoinDropChance) return;
            ref var pos = ref _world.GetPool<Position>().Get(enemy);
            _world.EventBus.CoinSpawnRequests.Raise(new SpawnRequest<CoinTag>(pos.X, pos.Z, enemy));
        }
    }
}