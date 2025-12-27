using _Project.Scripts.Core.ECS.Entity;

namespace _Project.Scripts.Infrastructure.EventBus.Events
{
    public readonly struct EntityDestroyedEvent
    {
        public readonly EntityId Entity;

        public EntityDestroyedEvent(EntityId entity)
        {
            Entity = entity;
        }
    }
}