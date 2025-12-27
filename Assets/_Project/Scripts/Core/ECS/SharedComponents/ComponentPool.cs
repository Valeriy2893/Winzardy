using System;
using _Project.Scripts.Core.ECS.Entity;

namespace _Project.Scripts.Core.ECS.SharedComponents
{
    public sealed class ComponentPool<T> : IComponentPool where T : struct
    {
        private T[] _components;
        private bool[] _has;
        private readonly World.World _world;
        public Type ComponentType => typeof(T);

        public ComponentPool(World.World world, int capacity = 128)
        {
            _world = world;
            _components = new T[capacity];
            _has = new bool[capacity];
        }

        public bool Has(EntityId entity) => entity.Value < _has.Length && _has[entity.Value];

        public void Add(EntityId entity, T component)
        {
            EnsureCapacity(entity.Value + 1);

            _components[entity.Value] = component;
            _has[entity.Value] = true;

            _world.OnComponentChanged(entity);
        }

        public void Remove(EntityId entity)
        {
            if (entity.Value >= _has.Length)
                return;

            _has[entity.Value] = false;
            _world.OnComponentChanged(entity);
        }
        
        public ref T Get(EntityId entity)
        {
            if (!Has(entity))
                throw new InvalidOperationException(
                    $"Component {typeof(T).Name} missing on entity {entity.Value}");

            return ref _components[entity.Value];
        }

        public void Set(EntityId entity, T value)
        {
            _components[entity.Value] = value;
            _has[entity.Value] = true;
            _world.OnComponentChanged(entity);
        }

        private void EnsureCapacity(int size)
        {
            if (size <= _components.Length)
                return;

            int newSize = Math.Max(_components.Length * 2, size);
            Array.Resize(ref _components, newSize);
            Array.Resize(ref _has, newSize);
        }

        public void Clear()
        {
            Array.Clear(_has, 0, _has.Length);
        }
#if UNITY_EDITOR
        public object GetBoxed(EntityId entity)
        {
            if (!Has(entity))
                return null;

            return _components[entity.Value];
        }
#endif
    }
}