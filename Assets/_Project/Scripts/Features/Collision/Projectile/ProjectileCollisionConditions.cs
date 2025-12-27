using _Project.Scripts.Core.ECS;
using _Project.Scripts.Core.ECS.Components;
using _Project.Scripts.Core.ECS.Entity;
using _Project.Scripts.Core.World;
using _Project.Scripts.Features.Condition;
using _Project.Scripts.Features.Enemy;

namespace _Project.Scripts.Features.Collision.Projectile
{
    public readonly struct ProjectileCollisionConditions : IEntityCondition
    {
        private readonly EcsFilter<EnemyTag, Position, CollisionRadius> _enemies;

        public ProjectileCollisionConditions(EcsFilter<EnemyTag, Position, CollisionRadius> enemies)
        {
            _enemies = enemies;
        }

        public bool IsMet(World world, float dt, EntityId projectile)
        {
            ref var pPos = ref world.GetPool<Position>().Get(projectile);
            ref var pRad = ref world.GetPool<CollisionRadius>().Get(projectile);

            foreach (var enemy in _enemies.Entities)
            {
                ref var ePos = ref world.GetPool<Position>().Get(enemy);
                ref var eRad = ref world.GetPool<CollisionRadius>().Get(enemy);

                float dx = pPos.X - ePos.X;
                float dz = pPos.Z - ePos.Z;
                float r = pRad.Value + eRad.Value;

                if (dx * dx + dz * dz <= r * r)
                    return true;
            }

            return false;
        }
    }
}