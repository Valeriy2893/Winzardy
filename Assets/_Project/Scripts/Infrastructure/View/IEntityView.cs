using _Project.Scripts.Core.ECS.Entity;
using _Project.Scripts.Core.World;

namespace _Project.Scripts.Infrastructure.View
{
    public interface IEntityView
    {
        public void Init(World world, EntityId entityId);
    }
}