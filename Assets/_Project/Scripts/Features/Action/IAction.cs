using _Project.Scripts.Core.ECS.Entity;
using _Project.Scripts.Core.World;

namespace _Project.Scripts.Features.Action
{
    public interface IAction
    {
        public void Execute(World world, EntityId entity, float dt);
    }
}