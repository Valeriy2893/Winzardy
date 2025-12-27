using _Project.Scripts.Core.World;

namespace _Project.Scripts.Features.Spawn
{
    public interface ISpawnFactory<TTag> where TTag : struct
    {
        public void Create(World world, in SpawnRequest<TTag> request);
    }
}