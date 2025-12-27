using _Project.Scripts.Core.ECS.Entity;
using _Project.Scripts.Core.World;

namespace _Project.Scripts.Features.Condition
{
    public interface IEntityCondition
    {
        public bool IsMet(World world, float dt, EntityId entity);
    }
}