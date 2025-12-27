using _Project.Scripts.Core.ECS.Entity;
using _Project.Scripts.Core.World;
using _Project.Scripts.Features.Player.ECS;

namespace _Project.Scripts.Features.Spawn.Player
{
    public readonly struct PlayerSpawnRequestBuilder : ISpawnRequestBuilder<PlayerTag>
    {
        public SpawnRequest<PlayerTag> Build(World world)
        {
            return new SpawnRequest<PlayerTag>(0, 0, new EntityId());
        }
    }
}