using System;
using _Project.Scripts.Core.ECS.Components;
using _Project.Scripts.Core.World;
using _Project.Scripts.Features.Enemy;
using _Project.Scripts.Features.EnemySpawner;

namespace _Project.Scripts.Features.Spawn.Enemy
{
    public readonly struct EnemySpawnRequestBuilder : ISpawnRequestBuilder<EnemyTag>
    {
        private readonly EnemyConfig _config;
        private readonly Random _random;

        public EnemySpawnRequestBuilder(EnemyConfig config, Random random)
        {
            _config = config;
            _random = random;
        }

        public SpawnRequest<EnemyTag> Build(World world)
        {
            ref var bounds = ref world.GetPool<ViewportBounds>().Get(world.GetFilter<ViewportBounds>().Entities[0]);
            bool left = _random.NextDouble() > 0.5f;
            float x = left ? bounds.MinX - _config.SpawnOffset : bounds.MaxX + _config.SpawnOffset;
            float z = bounds.MinZ + (bounds.MaxZ - bounds.MinZ) * (float)_random.NextDouble();
            var spawner = world.GetFilter<EnemySpawnerTag>().Entities[0];
            return new SpawnRequest<EnemyTag>(x, z, spawner);
        }
    }
}