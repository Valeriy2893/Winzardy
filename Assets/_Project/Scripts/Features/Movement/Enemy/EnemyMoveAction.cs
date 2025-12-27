using _Project.Scripts.Core.ECS;
using _Project.Scripts.Core.ECS.Components;
using _Project.Scripts.Core.World;
using _Project.Scripts.Features.Action;
using _Project.Scripts.Features.Player.ECS;
using _Project.Scripts.Infrastructure.EventBus.Events;
using UnityEngine;
using EntityId = _Project.Scripts.Core.ECS.Entity.EntityId;

namespace _Project.Scripts.Features.Movement.Enemy
{
    public readonly struct EnemyMoveAction : IAction
    {
        private readonly EcsFilter<PlayerTag, Position> _players;

        public EnemyMoveAction(EcsFilter<PlayerTag, Position> players)
        {
            _players = players;
        }

        public void Execute(World world, EntityId enemy, float dt)
        {
            if (_players.Entities.Count == 0)
                return;

            var player = _players.Entities[0];

            ref var playerPos = ref world.GetPool<Position>().Get(player);
            ref var pos = ref world.GetPool<Position>().Get(enemy);
            ref var dir = ref world.GetPool<Direction>().Get(enemy);
            ref var vel = ref world.GetPool<Velocity>().Get(enemy);
            ref var range = ref world.GetPool<AttackRange>().Get(enemy);

            float dx = playerPos.X - pos.X;
            float dz = playerPos.Z - pos.Z;
            float sqrDist = dx * dx + dz * dz;

            if (sqrDist <= range.Value * range.Value)
            {
                dir.X = 0f;
                dir.Z = 0f;
                return;
            }

            float invLen = 1f / Mathf.Sqrt(sqrDist);

            dir.X = dx * invLen;
            dir.Z = dz * invLen;

            pos.X += dir.X * vel.Speed * dt;
            pos.Z += dir.Z * vel.Speed * dt;

            world.EventBus.PositionChanged.Raise(new PositionChangedEvent(enemy, pos.X, pos.Z));
        }
    }
}