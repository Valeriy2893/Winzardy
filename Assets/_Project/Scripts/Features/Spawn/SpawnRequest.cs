using _Project.Scripts.Core.ECS.Entity;

namespace _Project.Scripts.Features.Spawn
{
    public readonly struct SpawnRequest<TTag> where TTag : struct
    {
        public readonly EntityId Source;
        public readonly float X;
        public readonly float Z;

        public SpawnRequest(float x, float z, EntityId source)
        {
            X = x;
            Z = z;
            Source = source;
        }
    }
}