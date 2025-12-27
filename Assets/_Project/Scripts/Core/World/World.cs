using System;
using System.Collections.Generic;
using _Project.Scripts.Core.ECS;
using _Project.Scripts.Core.ECS.Entity;
using _Project.Scripts.Core.ECS.SharedComponents;
using _Project.Scripts.Infrastructure.EventBus;
using _Project.Scripts.Infrastructure.EventBus.Events;

namespace _Project.Scripts.Core.World
{
    public sealed class World : IDisposable
    {
        private int _nextEntityId;
        private readonly EntitySet _aliveEntities = new();
        public readonly EventBus EventBus = new();
        private readonly ComponentTypeRegistry _componentTypes = new();
        private IComponentPool[] _pools = new IComponentPool[32];
        private readonly List<IEcsFilter> _filters = new();

        public EntityId CreateEntity()
        {
            var id = new EntityId(_nextEntityId++);
            _aliveEntities.Add(id);
            return id;
        }

        public void DestroyEntity(EntityId entity)
        {
            if (!_aliveEntities.Contains(entity))
                return;

            EventBus.EntityDestroyed.Raise(new EntityDestroyedEvent(entity));
            for (int i = 0; i < _pools.Length; i++)
                _pools[i]?.Remove(entity);

            _aliveEntities.Remove(entity);
        }

        public ComponentPool<T> GetPool<T>() where T : struct
        {
            int typeId = _componentTypes.GetId<T>();

            if (typeId >= _pools.Length)
                Array.Resize(ref _pools, Math.Max(_pools.Length * 2, typeId + 1));

            _pools[typeId] ??= new ComponentPool<T>(this);
            return (ComponentPool<T>)_pools[typeId];
        }

        public EcsFilter<T1> GetFilter<T1>() where T1 : struct
        {
            var filter = new EcsFilter<T1>(this);
            _filters.Add(filter);
            InitFilter(filter);
            return filter;
        }

        public EcsFilter<T1, T2> GetFilter<T1, T2>()
            where T1 : struct
            where T2 : struct
        {
            var filter = new EcsFilter<T1, T2>(this);
            _filters.Add(filter);
            InitFilter(filter);
            return filter;
        }

        public EcsFilter<T1, T2, T3> GetFilter<T1, T2, T3>()
            where T1 : struct
            where T2 : struct
            where T3 : struct
        {
            var filter = new EcsFilter<T1, T2, T3>(this);
            _filters.Add(filter);
            InitFilter(filter);
            return filter;
        }

        public EcsFilter<T1, T2, T3, T4> GetFilter<T1, T2, T3, T4>()
            where T1 : struct
            where T2 : struct
            where T3 : struct
            where T4 : struct
        {
            var filter = new EcsFilter<T1, T2, T3, T4>(this);
            _filters.Add(filter);
            InitFilter(filter);
            return filter;
        }

        private void InitFilter(IEcsFilter filter)
        {
            foreach (var entity in _aliveEntities.Entities)
                filter.OnEntityChanged(entity);
        }

        public void OnComponentChanged(EntityId entity)
        {
            for (int i = 0; i < _filters.Count; i++)
                _filters[i].OnEntityChanged(entity);
        }

        public void Dispose()
        {
            EventBus.Clear();

            foreach (var filter in _filters)
                filter.Clear();

            _filters.Clear();

            for (int i = 0; i < _pools.Length; i++)
                _pools[i]?.Clear();

            Array.Clear(_pools, 0, _pools.Length);
            _componentTypes.Clear();

            _aliveEntities.Clear();
            _nextEntityId = 0;
        }
#if UNITY_EDITOR
        public ReadOnlySpan<EntityId> DebugAliveEntities => _aliveEntities.Entities;

        public IEnumerable<IComponentPool> DebugComponentPools
        {
            get
            {
                for (int i = 0; i < _pools.Length; i++)
                    if (_pools[i] != null)
                        yield return _pools[i];
            }
        }
#endif
    }
}