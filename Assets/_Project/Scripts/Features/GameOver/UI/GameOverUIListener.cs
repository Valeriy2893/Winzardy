using _Project.Scripts.Core.World;
using _Project.Scripts.Features.Player.ECS;
using _Project.Scripts.Infrastructure.EventBus;
using _Project.Scripts.Infrastructure.EventBus.Events;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Features.GameOver.UI
{
    public class GameOverUIListener : IEventListener<EntityDestroyedEvent>
    {
        private readonly World _world;
        private readonly GameObject _panel;
        private readonly System.Action _restart;

        public GameOverUIListener(World world, GameObject panel, Button buttonRestart, System.Action restart)
        {
            _world = world;
            _panel = panel;
            _restart = restart;
            _panel.SetActive(false);
            
            buttonRestart.onClick.AddListener(() =>
            {
                Time.timeScale = 1f;
                _restart?.Invoke();
            });
        }

        public void OnEvent(in EntityDestroyedEvent evt)
        {
            var entity = evt.Entity;
            if (!_world.GetPool<PlayerTag>().Has(entity)) return;
            _panel.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}