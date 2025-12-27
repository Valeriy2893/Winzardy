using _Project.Scripts.Features.Coin;
using _Project.Scripts.Features.Enemy;
using _Project.Scripts.Features.EnemySpawner;
using _Project.Scripts.Features.Player.ECS;
using _Project.Scripts.Features.Projectile;
using _Project.Scripts.Features.Spawn;
using _Project.Scripts.Infrastructure.EventBus.Events;

namespace _Project.Scripts.Infrastructure.EventBus
{
    public sealed class EventBus
    {
        public readonly EventChannel<EntityCreatedEvent> EntityCreated = new();
        public readonly EventChannel<EntityDestroyedEvent> EntityDestroyed = new();
        public readonly EventChannel<PositionChangedEvent> PositionChanged = new();
        public readonly EventChannel<SpawnRequest<PlayerTag>> PlayerSpawnRequests = new();
        public readonly EventChannel<SpawnRequest<EnemyTag>> EnemySpawnRequests = new();
        public readonly EventChannel<SpawnRequest<EnemySpawnerTag>> EnemySpawnerRequests = new();
        public readonly EventChannel<SpawnRequest<CoinTag>> CoinSpawnRequests = new();
        public readonly EventChannel<SpawnRequest<ProjectileTag>> ProjectileSpawnRequests = new();

        public void Clear()
        {
            EntityCreated.Clear();
            EntityDestroyed.Clear();
            PositionChanged.Clear();
            PlayerSpawnRequests.Clear();
            EnemySpawnRequests.Clear();
            EnemySpawnerRequests.Clear();
            CoinSpawnRequests.Clear();
            ProjectileSpawnRequests.Clear();
        }
    }
}