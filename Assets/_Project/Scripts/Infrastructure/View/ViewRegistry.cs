using System.Collections.Generic;
using _Project.Scripts.Core.World;
using _Project.Scripts.Features.Coin;
using _Project.Scripts.Features.Enemy;
using _Project.Scripts.Features.Projectile;
using UnityEngine;
using EntityId = _Project.Scripts.Core.ECS.Entity.EntityId;

namespace _Project.Scripts.Infrastructure.View
{
    public sealed class ViewRegistry
    {
        private readonly Dictionary<int, GameObject> _views = new();
        private readonly ViewPool _projectilePool;
        private readonly ViewPool _enemyPool;
        private readonly ViewPool _coinPool;
        private readonly World _world;

        public ViewRegistry(ViewPool projectilePool, ViewPool enemyPool, ViewPool coinPool, World world)
        {
            _projectilePool = projectilePool;
            _enemyPool = enemyPool;
            _coinPool = coinPool;
            _world = world;
        }

        public bool Has(EntityId entity)
        {
            return _views.ContainsKey(entity.Value);
        }

        public GameObject Get(EntityId entity)
        {
            return _views[entity.Value];
        }

        public void Add(EntityId entity, GameObject view)
        {
            _views[entity.Value] = view;
        }

        public void Remove(EntityId entity)
        {
            if (!_views.TryGetValue(entity.Value, out var go))
                return;

            if (_world.GetPool<ProjectileTag>().Has(entity))
                _projectilePool.Release(go);
            else if (_world.GetPool<EnemyTag>().Has(entity))
                _enemyPool.Release(go);
            else if (_world.GetPool<CoinTag>().Has(entity))
                _coinPool.Release(go);
            else
                Object.Destroy(go);

            _views.Remove(entity.Value);
        }

#if UNITY_EDITOR
        public bool TryGet(EntityId entity, out GameObject view)
            => _views.TryGetValue(entity.Value, out view);
#endif
    }
}