using _Project.Scripts.Core.ECS.Entity;

namespace _Project.Scripts.Infrastructure.EventBus.Events
{
    public readonly struct PositionChangedEvent
    {
        public readonly EntityId Entity;
        public readonly float X;
        public readonly float Z;

        public PositionChangedEvent(EntityId entity, float x, float z)
        {
            Entity = entity;
            X = x;
            Z = z;
        }
    }
}