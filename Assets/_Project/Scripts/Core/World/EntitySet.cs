using System;
using _Project.Scripts.Core.ECS.Entity;

namespace _Project.Scripts.Core.World
{
    public sealed class EntitySet
    {
        private EntityId[] _dense = new EntityId[128];
        private int[] _sparse = new int[128];
        private int _count;

        public ReadOnlySpan<EntityId> Entities => _dense.AsSpan(0, _count);

        public void Add(EntityId entity)
        {
            int id = entity.Value;
            EnsureCapacity(id);

            _sparse[id] = _count;
            _dense[_count] = entity;
            _count++;
        }

        public void Remove(EntityId entity)
        {
            int id = entity.Value;
            int index = _sparse[id];
            int lastIndex = _count - 1;

            if (index != lastIndex)
            {
                var last = _dense[lastIndex];
                _dense[index] = last;
                _sparse[last.Value] = index;
            }

            _count--;
        }

        public bool Contains(EntityId entity)
        {
            int id = entity.Value;
            if (id >= _sparse.Length)
                return false;

            int index = _sparse[id];
            return index < _count && _dense[index].Value == id;
        }

        private void EnsureCapacity(int id)
        {
            if (id < _sparse.Length)
                return;

            int newSize = Math.Max(id + 1, _sparse.Length * 2);
            Array.Resize(ref _sparse, newSize);
            Array.Resize(ref _dense, newSize);
        }

        public void Clear()
        {
            _count = 0;
        }
    }
}