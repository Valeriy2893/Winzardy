using System.Collections.Generic;
using _Project.Scripts.Core.ECS.Entity;
using _Project.Scripts.Core.ECS.SharedComponents;

namespace _Project.Scripts.Core.ECS
{
    public sealed class EcsFilter<T1> : IEcsFilter
        where T1 : struct
    {
        private readonly ComponentPool<T1> _p1;
        private readonly List<EntityId> _entities = new();

        public IReadOnlyList<EntityId> Entities => _entities;

        public EcsFilter(World.World world)
        {
            _p1 = world.GetPool<T1>();
        }

        public void OnEntityChanged(EntityId e)
        {
            bool has = _p1.Has(e);
            bool contains = _entities.Contains(e);

            if (has && !contains)
                _entities.Add(e);
            else if (!has && contains)
                _entities.Remove(e);
        }

        public void Clear()
        {
            _entities.Clear();
        }
    }


    public sealed class EcsFilter<T1, T2> : IEcsFilter
        where T1 : struct
        where T2 : struct
    {
        private readonly ComponentPool<T1> _p1;
        private readonly ComponentPool<T2> _p2;
        private readonly List<EntityId> _entities = new();

        public IReadOnlyList<EntityId> Entities => _entities;

        public EcsFilter(World.World world)
        {
            _p1 = world.GetPool<T1>();
            _p2 = world.GetPool<T2>();
        }

        public void OnEntityChanged(EntityId e)
        {
            bool has =
                _p1.Has(e) &&
                _p2.Has(e);

            bool contains = _entities.Contains(e);

            if (has && !contains)
                _entities.Add(e);
            else if (!has && contains)
                _entities.Remove(e);
        }

        public void Clear()
        {
            _entities.Clear();
        }
    }


    public sealed class EcsFilter<T1, T2, T3> : IEcsFilter
        where T1 : struct
        where T2 : struct
        where T3 : struct
    {
        private readonly ComponentPool<T1> _p1;
        private readonly ComponentPool<T2> _p2;
        private readonly ComponentPool<T3> _p3;

        private readonly List<EntityId> _entities = new();
        public IReadOnlyList<EntityId> Entities => _entities;

        public EcsFilter(World.World world)
        {
            _p1 = world.GetPool<T1>();
            _p2 = world.GetPool<T2>();
            _p3 = world.GetPool<T3>();
        }

        public void OnEntityChanged(EntityId e)
        {
            bool has =
                _p1.Has(e) &&
                _p2.Has(e) &&
                _p3.Has(e);

            bool contains = _entities.Contains(e);

            if (has && !contains)
                _entities.Add(e);
            else if (!has && contains)
                _entities.Remove(e);
        }

        public void Clear()
        {
            _entities.Clear();
        }
    }


    public sealed class EcsFilter<T1, T2, T3, T4> : IEcsFilter
        where T1 : struct
        where T2 : struct
        where T3 : struct
        where T4 : struct
    {
        private readonly ComponentPool<T1> _p1;
        private readonly ComponentPool<T2> _p2;
        private readonly ComponentPool<T3> _p3;
        private readonly ComponentPool<T4> _p4;

        private readonly List<EntityId> _entities = new();
        public IReadOnlyList<EntityId> Entities => _entities;

        public EcsFilter(World.World world)
        {
            _p1 = world.GetPool<T1>();
            _p2 = world.GetPool<T2>();
            _p3 = world.GetPool<T3>();
            _p4 = world.GetPool<T4>();
        }

        public void OnEntityChanged(EntityId e)
        {
            bool has = _p1.Has(e) && _p2.Has(e) && _p3.Has(e) && _p4.Has(e);
            bool contains = _entities.Contains(e);

            if (has && !contains)
                _entities.Add(e);
            else if (!has && contains)
                _entities.Remove(e);
        }

        public void Clear()
        {
            _entities.Clear();
        }
    }
}