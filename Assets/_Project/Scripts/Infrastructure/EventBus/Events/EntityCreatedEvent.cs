using _Project.Scripts.Core.ECS.Entity;

namespace _Project.Scripts.Infrastructure.EventBus.Events
{
    public readonly struct EntityCreatedEvent
    {
        public readonly EntityId Entity;

        public EntityCreatedEvent(EntityId entity)
        {
            Entity = entity;
        }
    }
}