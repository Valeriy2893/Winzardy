using _Project.Scripts.Core.ECS;
using _Project.Scripts.Core.ECS.Components;
using _Project.Scripts.Core.World;
using _Project.Scripts.Features.Player.ECS;
using _Project.Scripts.Features.Projectile;

namespace _Project.Scripts.Features.Spawn.Projectile
{
    public readonly struct PlayerProjectileSpawnRequestBuilder : ISpawnRequestBuilder<ProjectileTag>
    {
        private readonly EcsFilter<PlayerTag, Position> _players;

        public PlayerProjectileSpawnRequestBuilder(EcsFilter<PlayerTag, Position> players)
        {
            _players = players;
        }

        public SpawnRequest<ProjectileTag> Build(World world)
        {
            if (_players.Entities.Count == 0) return default;
            var player = _players.Entities[0];
            ref var pos = ref world.GetPool<Position>().Get(player);
            return new SpawnRequest<ProjectileTag>(pos.X, pos.Z, player);
        }
    }
}