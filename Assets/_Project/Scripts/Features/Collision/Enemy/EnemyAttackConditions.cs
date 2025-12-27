using _Project.Scripts.Core.ECS;
using _Project.Scripts.Core.ECS.Components;
using _Project.Scripts.Core.ECS.Entity;
using _Project.Scripts.Core.World;
using _Project.Scripts.Features.Condition;
using _Project.Scripts.Features.Player.ECS;

namespace _Project.Scripts.Features.Collision.Enemy
{
    public readonly struct EnemyAttackConditions : IEntityCondition
    {
        private readonly EcsFilter<PlayerTag, Position> _player;

        public EnemyAttackConditions(EcsFilter<PlayerTag, Position> player)
        {
            _player = player;
        }

        public bool IsMet(World world, float dt, EntityId enemy)
        {
            if (_player.Entities.Count == 0)
                return false;

            var player = _player.Entities[0];

            ref var ePos = ref world.GetPool<Position>().Get(enemy);
            ref var range = ref world.GetPool<AttackRange>().Get(enemy);

            ref var pPos = ref world.GetPool<Position>().Get(player);

            float dx = pPos.X - ePos.X;
            float dz = pPos.Z - ePos.Z;

            return dx * dx + dz * dz <= range.Value * range.Value;
        }
    }
}