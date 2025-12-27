using _Project.Scripts.Infrastructure.EventBus;
using _Project.Scripts.Infrastructure.EventBus.Events;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.View
{
    public class ViewPositionListener : IEventListener<PositionChangedEvent>
    {
        private readonly ViewRegistry _views;

        public ViewPositionListener(ViewRegistry view)
        {
            _views = view;
        }

        public void OnEvent(in PositionChangedEvent evt)
        {
            if (!_views.Has(evt.Entity))
                return;

            var go = _views.Get(evt.Entity);
           go.transform.position = new Vector3(evt.X, go.transform.position.y, evt.Z);
        }
    }
}