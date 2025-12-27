using _Project.Scripts.Core.ECS.Components;
using _Project.Scripts.Core.World;
using _Project.Scripts.Features.Player;
using _Project.Scripts.Features.Player.ECS;
using _Project.Scripts.Infrastructure.EventBus.Events;

namespace _Project.Scripts.Features.Spawn.Player
{
    public readonly struct PlayerSpawnFactory : ISpawnFactory<PlayerTag>
    {
        private readonly PlayerConfig _config;

        public PlayerSpawnFactory(PlayerConfig config)
        {
            _config = config;
        }

        public void Create(World world, in SpawnRequest<PlayerTag> request)
        {
            var player = world.CreateEntity();
            world.GetPool<Position>().Add(player, new Position { X = 0f, Z = 0f });
            world.GetPool<Velocity>().Add(player, new Velocity { Speed = _config.MoveSpeed });
            world.GetPool<Direction>().Add(player, new Direction());
            world.GetPool<Health>().Add(player, new Health { Current = _config.MaxHealth, Max = _config.MaxHealth });
            world.GetPool<PlayerTag>().Add(player, new PlayerTag());
            world.GetPool<Timer>().Add(player, new Timer
            {
                Interval = _config.ShootInterval,
                TimeLeft = _config.ShootInterval
            });
            world.GetPool<Resource>().Add(player, new Resource { Value = 0 });
            world.GetPool<CollisionRadius>().Add(player, new CollisionRadius
            {
                Value = _config.CollisionRadius
            });
            world.EventBus.EntityCreated.Raise(new EntityCreatedEvent(player));
        }
    }
}