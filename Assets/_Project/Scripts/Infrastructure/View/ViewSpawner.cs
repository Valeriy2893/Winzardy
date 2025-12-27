using _Project.Scripts.Core.ECS.Components;
using _Project.Scripts.Core.World;
using _Project.Scripts.Infrastructure.EventBus;
using _Project.Scripts.Infrastructure.EventBus.Events;
using UnityEngine;
using EntityId = _Project.Scripts.Core.ECS.Entity.EntityId;

namespace _Project.Scripts.Infrastructure.View
{
    public sealed class ViewSpawner<TTag> : IEventListener<EntityCreatedEvent> where TTag : struct
    {
        private readonly World _world;
        private readonly ViewRegistry _views;
        private readonly ViewPool _pool;
        private readonly GameObject _oneEntity;
        public GameObject Entity { get; private set; }

        public ViewSpawner(World world, ViewRegistry views, ViewPool pool)
        {
            _world = world;
            _views = views;
            _pool = pool;
        }

        public ViewSpawner(World world, ViewRegistry views, GameObject oneEntity)
        {
            _world = world;
            _views = views;
            _oneEntity = oneEntity;
        }

        public void OnEvent(in EntityCreatedEvent evt)
        {
            var entity = evt.Entity;
            if (!_world.GetPool<TTag>().Has(entity)) return;
            if (_views.Has(entity)) return;
            Spawn(entity);
        }

        private void Spawn(EntityId entity)
        {
            Entity = _oneEntity != null ? Object.Instantiate(_oneEntity) : _pool.Get();
            _views.Add(entity, Entity);

            if (_world.GetPool<Position>().Has(entity))
            {
                ref var pos = ref _world.GetPool<Position>().Get(entity);
                Entity.transform.position = new Vector3(pos.X, 0f, pos.Z);
            }

            if (Entity.TryGetComponent<IEntityView>(out var view))
                view.Init(_world, entity);
        }
    }
}