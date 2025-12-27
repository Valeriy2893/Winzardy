using _Project.Scripts.Core.ECS;
using _Project.Scripts.Core.ECS.Components;
using _Project.Scripts.Core.ECS.Entity;
using _Project.Scripts.Core.World;
using _Project.Scripts.Features.Coin;
using _Project.Scripts.Features.Condition;
using _Project.Scripts.Features.Player.ECS;

namespace _Project.Scripts.Features.Collision.Coin
{
    public readonly struct CoinPickupConditions : IEntityCondition
    {
        private readonly EcsFilter<PlayerTag, Position, CollisionRadius> _players;

        public CoinPickupConditions(EcsFilter<PlayerTag, Position, CollisionRadius> players)
        {
            _players = players;
        }

        public bool IsMet(World world, float dt, EntityId coin)
        {
            if (_players.Entities.Count == 0)
                return false;
            
            if (!world.GetPool<CoinTag>().Has(coin))
                return false;
            
            var player = _players.Entities[0];

            ref var coinPos = ref world.GetPool<Position>().Get(coin);
            ref var coinRad = ref world.GetPool<CollisionRadius>().Get(coin);

            ref var playerPos = ref world.GetPool<Position>().Get(player);
            ref var playerRad = ref world.GetPool<CollisionRadius>().Get(player);

            float dx = playerPos.X - coinPos.X;
            float dz = playerPos.Z - coinPos.Z;
            float r = playerRad.Value + coinRad.Value;

            return dx * dx + dz * dz <= r * r;
        }
    }
}