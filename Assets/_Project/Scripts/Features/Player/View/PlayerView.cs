using _Project.Scripts.Core.ECS.Components;
using _Project.Scripts.Core.World;
using _Project.Scripts.Infrastructure.View;
using UnityEngine;
using EntityId = _Project.Scripts.Core.ECS.Entity.EntityId;

namespace _Project.Scripts.Features.Player.View
{
    public sealed class PlayerView : MonoBehaviour, IEntityView
    {
        private EntityId _entity;
        private World _world;

        public void Init(World world, EntityId entityId)
        {
            _entity = entityId;
            _world = world;
            GetComponentInChildren<CameraViewportAdapter>().Init(world);
        }

        public void Update()
        {
            if (_world == null) return;
            
            ref var pos = ref _world.GetPool<Position>().Get(_entity);
            transform.position = new Vector3(pos.X, 0f, pos.Z);
        }
    }
}