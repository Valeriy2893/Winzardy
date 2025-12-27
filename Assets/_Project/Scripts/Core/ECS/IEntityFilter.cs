using System.Collections.Generic;
using _Project.Scripts.Core.ECS.Entity;

namespace _Project.Scripts.Core.ECS
{
    public interface IEcsFilter
    {
        public void OnEntityChanged(EntityId entity);
        public void Clear();
    }
}