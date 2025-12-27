using _Project.Scripts.Core.ECS.Components;
using _Project.Scripts.Core.World;
using TMPro;
using UnityEngine;
using EntityId = _Project.Scripts.Core.ECS.Entity.EntityId;

namespace _Project.Scripts.Features.Enemy
{
    public sealed class EnemyHealthView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        private World _world;
        private EntityId _entity;

        public void Init(World world, EntityId entity)
        {
            _world = world;
            _entity = entity;
        }

        private void LateUpdate()
        {
            if (_world == null) return;
            if (!_world.GetPool<Health>().Has(_entity))
                return;

            var hp = _world.GetPool<Health>().Get(_entity);
            _text.text = ((int)hp.Current).ToString();
        }
    }
}