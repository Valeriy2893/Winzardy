using _Project.Scripts.Infrastructure.EventBus;
using _Project.Scripts.Infrastructure.EventBus.Events;

namespace _Project.Scripts.Infrastructure.View
{
    public sealed class ViewCleanupListener : IEventListener<EntityDestroyedEvent>
    {
        private readonly ViewRegistry _views;

        public ViewCleanupListener(ViewRegistry views)
        {
            _views = views;
        }

        public void OnEvent(in EntityDestroyedEvent evt)
        {
            _views.Remove(evt.Entity);
        }
    }
}