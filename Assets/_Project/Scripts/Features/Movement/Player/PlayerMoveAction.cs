using _Project.Scripts.Core.ECS.Components;
using _Project.Scripts.Core.World;
using _Project.Scripts.Features.Action;
using _Project.Scripts.Infrastructure.EventBus.Events;
using UnityEngine;
using EntityId = _Project.Scripts.Core.ECS.Entity.EntityId;

namespace _Project.Scripts.Features.Movement.Player
{
    public readonly struct PlayerMoveAction : IAction
    {
        public void Execute(World world, EntityId entity, float dt)
        {
            ref var pos = ref world.GetPool<Position>().Get(entity);
            ref var vel = ref world.GetPool<Velocity>().Get(entity);
            ref var dir = ref world.GetPool<Direction>().Get(entity);
            
            var d = new Vector2(dir.X, dir.Z);
            if (d.sqrMagnitude > 1f)
                d.Normalize();

            var oldX = pos.X;
            var oldZ = pos.Z;

            pos.X += d.x * vel.Speed * dt;
            pos.Z += d.y * vel.Speed * dt;
            if (!Mathf.Approximately(pos.X, oldX) ||
                !Mathf.Approximately(pos.Z, oldZ))
            {
                world.EventBus.PositionChanged.Raise(new PositionChangedEvent(entity, pos.X, pos.Z));
            }
        }
    }
}