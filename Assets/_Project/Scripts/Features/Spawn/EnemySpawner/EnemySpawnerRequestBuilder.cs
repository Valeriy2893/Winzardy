using System;
using _Project.Scripts.Core.ECS.Components;
using _Project.Scripts.Core.ECS.Entity;
using _Project.Scripts.Core.World;
using _Project.Scripts.Features.EnemySpawner;

namespace _Project.Scripts.Features.Spawn.EnemySpawner
{
    public readonly struct EnemySpawnerRequestBuilder : ISpawnRequestBuilder<EnemySpawnerTag>
    {
        private readonly EnemySpawnerConfig _config;
        private readonly Random _random;

        public EnemySpawnerRequestBuilder(EnemySpawnerConfig config, Random random)
        {
            _config = config;
            _random = random;
        }

        public SpawnRequest<EnemySpawnerTag> Build(World world)
        {
            ref var bounds = ref world.GetPool<ViewportBounds>().Get(world.GetFilter<ViewportBounds>().Entities[0]);
            bool left = _random.NextDouble() > 0.5f;
            float x = left ? bounds.MinX - _config.SpawnOffset : bounds.MaxX + _config.SpawnOffset;
            float z = bounds.MinZ + (bounds.MaxZ - bounds.MinZ) * (float)_random.NextDouble();
            return new SpawnRequest<EnemySpawnerTag>(x, z, new EntityId());
        }
    }
}