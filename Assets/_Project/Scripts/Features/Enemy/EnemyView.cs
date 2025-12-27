using _Project.Scripts.Core.World;
using _Project.Scripts.Infrastructure.View;
using UnityEngine;
using EntityId = _Project.Scripts.Core.ECS.Entity.EntityId;

namespace _Project.Scripts.Features.Enemy
{
    public sealed class EnemyView : MonoBehaviour, IEntityView
    {
        public void Init(World world, EntityId entityId)
        {
            var hpView = gameObject.GetComponent<EnemyHealthView>();
            hpView.Init(world, entityId);
        }
    }
}