using _Project.Scripts.Core.ECS;
using _Project.Scripts.Core.ECS.Components;
using _Project.Scripts.Core.World;
using _Project.Scripts.Features.Player.ECS;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.Features.Player.UI
{
    public sealed class PlayerHealthUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        private World _world;
        private EcsFilter<PlayerTag, Health> _players;

        public void Init(World world)
        {
            _world = world;
            _players = world.GetFilter<PlayerTag, Health>();
        }

        private void Update()
        {
            if (_players.Entities.Count == 0)
                return;

            var player = _players.Entities[0];
            ref var health = ref _world.GetPool<Health>().Get(player);

            _text.text = $"{(int)health.Current}";
        }
    }
}