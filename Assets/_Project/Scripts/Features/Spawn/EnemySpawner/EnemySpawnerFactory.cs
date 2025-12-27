using _Project.Scripts.Core.ECS.Components;
using _Project.Scripts.Core.World;
using _Project.Scripts.Features.EnemySpawner;
using _Project.Scripts.Infrastructure.EventBus.Events;

namespace _Project.Scripts.Features.Spawn.EnemySpawner
{
    public readonly struct EnemySpawnerFactory : ISpawnFactory<EnemySpawnerTag>
    {
        private readonly EnemySpawnerConfig _config;

        public EnemySpawnerFactory(EnemySpawnerConfig config)
        {
            _config = config;
        }

        public void Create(World world, in SpawnRequest<EnemySpawnerTag> request)
        {
            var spawner = world.CreateEntity();
            world.GetPool<Position>().Add(spawner, new Position { X = request.X, Z = request.Z });
            world.GetPool<Timer>().Add(spawner, new Timer
            {
                Interval = _config.SpawnInterval,
                TimeLeft = _config.SpawnInterval
            });
            world.GetPool<EnemySpawnerTag>().Add(spawner, default);
            world.EventBus.EntityCreated.Raise(new EntityCreatedEvent(spawner));
        }
    }
}