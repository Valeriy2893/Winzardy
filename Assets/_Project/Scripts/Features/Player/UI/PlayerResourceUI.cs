using _Project.Scripts.Core.ECS;
using _Project.Scripts.Core.ECS.Components;
using _Project.Scripts.Core.World;
using _Project.Scripts.Features.Player.ECS;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.Features.Player.UI
{
    public sealed class PlayerResourceUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        private World _world;
        private EcsFilter<PlayerTag, Resource> _players;

        public void Init(World world)
        {
            _world = world;
            _players = world.GetFilter<PlayerTag, Resource>();
        }

        private void Update()
        {
            if (_players.Entities.Count == 0)
                return;

            var player = _players.Entities[0];
            ref var resource = ref _world.GetPool<Resource>().Get(player);

            _text.text = $"{resource.Value}";
        }
    }
}