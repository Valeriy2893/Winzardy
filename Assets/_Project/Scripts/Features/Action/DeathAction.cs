using _Project.Scripts.Core.World;
using EntityId = _Project.Scripts.Core.ECS.Entity.EntityId;

namespace _Project.Scripts.Features.Action
{
    public readonly struct DeathAction : IAction
    {
        public void Execute(World world, EntityId entity, float dt)
        {
            world.DestroyEntity(entity);
            
        }
    }
}