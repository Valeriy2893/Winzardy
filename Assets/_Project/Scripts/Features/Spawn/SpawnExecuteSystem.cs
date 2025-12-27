using _Project.Scripts.Core.World;
using _Project.Scripts.Infrastructure.EventBus;

namespace _Project.Scripts.Features.Spawn
{
    public sealed class SpawnExecuteSystem<TTag, TFactory> : IEventListener<SpawnRequest<TTag>>
        where TTag : struct
        where TFactory : struct, ISpawnFactory<TTag>
    {
        private readonly World _world;
        private TFactory _factory;

        public SpawnExecuteSystem(World world, TFactory factory)
        {
            _world = world;
            _factory = factory;
        }

        public void OnEvent(in SpawnRequest<TTag> request)
        {
            _factory.Create(_world, request);
        }
    }
}