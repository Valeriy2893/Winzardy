using _Project.Scripts.Core.ECS;
using _Project.Scripts.Core.ECS.Components;
using _Project.Scripts.Core.ECS.Entity;
using _Project.Scripts.Core.World;
using _Project.Scripts.Features.Action;
using _Project.Scripts.Features.Player.ECS;

namespace _Project.Scripts.Features.Collision.Enemy
{
    public readonly struct EnemyAttackAction : IAction
    {
        private readonly EcsFilter<PlayerTag, Health> _player;

        public EnemyAttackAction(EcsFilter<PlayerTag, Health> player)
        {
            _player = player;
        }

        public void Execute(World world, EntityId enemy, float dt)
        {
            if (_player.Entities.Count == 0)
                return;

            var player = _player.Entities[0];
            ref var health = ref world.GetPool<Health>().Get(player);
            ref var damage = ref world.GetPool<DamageOverTime>().Get(enemy);
            health.Current -= damage.DamagePerSecond * dt;
        }
    }
}