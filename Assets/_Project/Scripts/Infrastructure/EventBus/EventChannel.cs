using System;

namespace _Project.Scripts.Infrastructure.EventBus
{
    public sealed class EventChannel<T> where T : struct
    {
        private IEventListener<T>[] _listeners;
        private int _count;

        public EventChannel(int capacity = 8)
        {
            _listeners = new IEventListener<T>[capacity];
            _count = 0;
        }

        public void Subscribe(IEventListener<T> listener)
        {
            for (int i = 0; i < _count; i++)
                if (_listeners[i] == listener)
                    return;

            if (_count == _listeners.Length)
                Grow();

            _listeners[_count++] = listener;
        }

        public void Raise(in T evt)
        {
            for (int i = 0; i < _count; i++)
                _listeners[i].OnEvent(in evt);
        }

        private void Grow()
        {
            var newArray = new IEventListener<T>[_listeners.Length * 2];
            Array.Copy(_listeners, newArray, _listeners.Length);
            _listeners = newArray;
        }

        public void Clear()
        {
            for (int i = 0; i < _count; i++)
                _listeners[i] = null;

            _count = 0;
        }
    }
}