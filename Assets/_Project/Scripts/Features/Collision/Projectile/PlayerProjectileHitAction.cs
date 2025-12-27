using _Project.Scripts.Core.ECS;
using _Project.Scripts.Core.ECS.Components;
using _Project.Scripts.Core.ECS.Entity;
using _Project.Scripts.Core.World;
using _Project.Scripts.Features.Action;
using _Project.Scripts.Features.Enemy;

namespace _Project.Scripts.Features.Collision.Projectile
{
    public readonly struct PlayerProjectileHitAction : IAction
    {
        private readonly EcsFilter<EnemyTag, Position, CollisionRadius, Health> _enemies;

        public PlayerProjectileHitAction(EcsFilter<EnemyTag, Position, CollisionRadius, Health> enemies)
        {
            _enemies = enemies;
        }

        public void Execute(World world, EntityId projectile, float dt)
        {
            ref var pPos = ref world.GetPool<Position>().Get(projectile);
            ref var pRad = ref world.GetPool<CollisionRadius>().Get(projectile);
            ref var dmg = ref world.GetPool<Damage>().Get(projectile);

            foreach (var enemy in _enemies.Entities)
            {
                ref var ePos = ref world.GetPool<Position>().Get(enemy);
                ref var eRad = ref world.GetPool<CollisionRadius>().Get(enemy);
                ref var hp = ref world.GetPool<Health>().Get(enemy);

                float dx = pPos.X - ePos.X;
                float dz = pPos.Z - ePos.Z;
                float r = pRad.Value + eRad.Value;

                if (dx * dx > r * r || dz * dz > r * r)
                    continue;

                hp.Current -= dmg.Value;
                world.DestroyEntity(projectile);
                break;
            }
        }
    }
}