using _Project.Scripts.Core.ECS.Components;
using _Project.Scripts.Core.World;
using _Project.Scripts.Features.Player;
using _Project.Scripts.Features.Projectile;
using _Project.Scripts.Infrastructure.EventBus.Events;
using UnityEngine;

namespace _Project.Scripts.Features.Spawn.Projectile
{
    public readonly struct PlayerProjectileSpawnFactory : ISpawnFactory<ProjectileTag>
    {
        private readonly PlayerConfig _config;

        public PlayerProjectileSpawnFactory(PlayerConfig config)
        {
            _config = config;
        }

        public void Create(World world, in SpawnRequest<ProjectileTag> request)
        {
            var projectile = world.CreateEntity();

            ref var shooterPos = ref world.GetPool<Position>().Get(request.Source);
            var dir = Random.insideUnitCircle.normalized;
            world.GetPool<Position>().Add(projectile, new Position { X = shooterPos.X, Z = shooterPos.Z });
            world.GetPool<Direction>().Add(projectile, new Direction { X = dir.x, Z = dir.y });
            world.GetPool<Velocity>().Add(projectile, new Velocity { Speed = _config.ProjectileSpeed });
            world.GetPool<Damage>().Add(projectile, new Damage { Value = _config.ProjectileDamage });
            world.GetPool<ProjectileTag>().Add(projectile, default);
            world.GetPool<CollisionRadius>().Add(projectile, new CollisionRadius { Value = _config.ProjectileRadius });
            world.GetPool<Lifetime>().Add(projectile, new Lifetime { TimeLeft = _config.ProjectileLifetime });
            world.EventBus.EntityCreated.Raise(new EntityCreatedEvent(projectile));
        }
    }
}