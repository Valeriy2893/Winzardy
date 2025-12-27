using _Project.Scripts.Core.ECS.Components;
using _Project.Scripts.Core.World;
using _Project.Scripts.Features.Action;
using _Project.Scripts.Infrastructure.EventBus.Events;
using EntityId = _Project.Scripts.Core.ECS.Entity.EntityId;

namespace _Project.Scripts.Features.Movement.Projectile
{
    public readonly struct ProjectileMoveAction : IAction
    {
        public void Execute(World world, EntityId entity, float dt)
        {
            ref var pos = ref world.GetPool<Position>().Get(entity);
            ref var dir = ref world.GetPool<Direction>().Get(entity);
            ref var vel = ref world.GetPool<Velocity>().Get(entity);

            pos.X += dir.X * vel.Speed * dt;
            pos.Z += dir.Z * vel.Speed * dt;

            world.EventBus.PositionChanged.Raise(new PositionChangedEvent(entity, pos.X, pos.Z));
        }
    }
}