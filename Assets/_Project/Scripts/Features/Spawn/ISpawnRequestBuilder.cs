using _Project.Scripts.Core.World;

namespace _Project.Scripts.Features.Spawn
{
    public interface ISpawnRequestBuilder<TTag> where TTag : struct
    {
        public SpawnRequest<TTag> Build(World world);
    }
}