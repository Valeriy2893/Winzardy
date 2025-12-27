using _Project.Scripts.Core.ECS;
using _Project.Scripts.Core.ECS.Components;
using _Project.Scripts.Core.ECS.Entity;
using _Project.Scripts.Core.World;
using _Project.Scripts.Features.Action;
using _Project.Scripts.Features.Player.ECS;

namespace _Project.Scripts.Features.Collision.Coin
{
    public readonly struct CoinPickupAction : IAction
    {
        private readonly EcsFilter<PlayerTag, Resource> _players;

        public CoinPickupAction(EcsFilter<PlayerTag, Resource> players)
        {
            _players = players;
        }

        public void Execute(World world, EntityId coin, float dt)
        {
            if (_players.Entities.Count == 0)
                return;

            var player = _players.Entities[0];

            ref var resource = ref world.GetPool<Resource>().Get(player);
            resource.Value += 1;

            world.DestroyEntity(coin);
        }
    }
}