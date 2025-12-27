using _Project.Scripts.Core.ECS.Components;
using _Project.Scripts.Core.World;
using UnityEngine;
using EntityId = _Project.Scripts.Core.ECS.Entity.EntityId;

namespace _Project.Scripts.Infrastructure.View
{
    public sealed class CameraViewportAdapter : MonoBehaviour
    {
        [SerializeField] private Camera _camera;

        private World _world;
        private EntityId _entity;

        public void Init(World world)
        {
            _world = world;
            _entity = world.CreateEntity();
            world.GetPool<ViewportBounds>().Add(_entity, default);
        }

        private void LateUpdate()
        {
            if (_world == null)
                return;

            var bounds = CalculateBounds();
            _world.GetPool<ViewportBounds>().Set(_entity, bounds);
        }

        private ViewportBounds CalculateBounds()
        {
            float height = _camera.orthographicSize;
            float width = height * _camera.aspect;

            var pos = _camera.transform.position;

            return new ViewportBounds
            {
                MinX = pos.x - width,
                MaxX = pos.x + width,
                MinZ = pos.z - height,
                MaxZ = pos.z + height
            };
        }
    }
}