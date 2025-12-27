using _Project.Scripts.Core.ECS.Components;
using _Project.Scripts.Core.World;
using _Project.Scripts.Features.Enemy;
using _Project.Scripts.Infrastructure.EventBus.Events;

namespace _Project.Scripts.Features.Spawn.Enemy
{
    public readonly struct EnemySpawnFactory : ISpawnFactory<EnemyTag>
    {
        private readonly EnemyConfig _config;

        public EnemySpawnFactory(EnemyConfig config)
        {
            _config = config;
        }

        public void Create(World world, in SpawnRequest<EnemyTag> request)
        {
            var enemy = world.CreateEntity();
            world.GetPool<EnemyTag>().Add(enemy, default);
            world.GetPool<Position>().Add(enemy, new Position { X = request.X, Z = request.Z });
            world.GetPool<Direction>().Add(enemy, default);
            world.GetPool<Velocity>().Add(enemy, new Velocity { Speed = _config.MoveSpeed });
            world.GetPool<Health>().Add(enemy, new Health { Current = _config.MaxHealth, Max = _config.MaxHealth });
            world.GetPool<AttackRange>().Add(enemy, new AttackRange { Value = _config.AttackRange });
            world.GetPool<DamageOverTime>()
                .Add(enemy, new DamageOverTime { DamagePerSecond = _config.DamagePerSecond });
            world.GetPool<CollisionRadius>().Add(enemy, new CollisionRadius { Value = _config.CollisionRadius });
            world.EventBus.EntityCreated.Raise(new EntityCreatedEvent(enemy));
        }
    }
}