using System;
using _Project.Scripts.Core.ECS.Entity;

namespace _Project.Scripts.Core.ECS.SharedComponents
{
    public interface IComponentPool
    {
        public bool Has(EntityId entity);
        public void Remove(EntityId entity);
        public void Clear();
        public Type ComponentType { get; }
#if UNITY_EDITOR
        object GetBoxed(EntityId entity);
#endif
    }
}