using _Project.Scripts.Core.ECS.Components;
using _Project.Scripts.Core.World;
using _Project.Scripts.Features.Coin;
using _Project.Scripts.Infrastructure.EventBus.Events;

namespace _Project.Scripts.Features.Spawn.Coin
{
    public readonly struct CoinSpawnFactory : ISpawnFactory<CoinTag>
    {
        private readonly CoinConfig _coinConfig;

        public CoinSpawnFactory(CoinConfig coinConfig)
        {
            _coinConfig = coinConfig;
        }

        public void Create(World world, in SpawnRequest<CoinTag> request)
        {
            var coin = world.CreateEntity();
            world.GetPool<Position>().Add(coin, new Position { X = request.X, Z = request.Z });
            world.GetPool<CoinTag>().Add(coin, new CoinTag());
            world.GetPool<CollisionRadius>().Add(coin, new CollisionRadius { Value = _coinConfig.CollisionRadius });
            world.EventBus.EntityCreated.Raise(new EntityCreatedEvent(coin));
        }
    }
}