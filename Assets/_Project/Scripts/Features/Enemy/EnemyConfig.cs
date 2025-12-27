using UnityEngine;

namespace _Project.Scripts.Features.Enemy
{
    [CreateAssetMenu(menuName = "Configs/Enemy Config")]
    public sealed class EnemyConfig : ScriptableObject
    {
        [Header("Health")]
        [field: SerializeField]
        public float MaxHealth { get; private set; } = 100f;

        [Header("Movement")]
        [field: SerializeField]
        public float MoveSpeed { get; private set; } = 3f;

        [Header("Attack")]
        [field: SerializeField]
        public float DamagePerSecond { get; private set; } = 5f;

        [field: SerializeField] public float AttackRange { get; private set; } = 1.5f;

        [Header("Spawn")]
        [field: SerializeField]
        public float SpawnOffset { get; private set; } = 2f;

        [Header("Collision")]
        [field: SerializeField]
        public float CollisionRadius { get; private set; } = 0.6f;

        [Header("Loot")]
        [Range(0f, 1f)]
        [field: SerializeField]
        public float CoinDropChance { get; private set; } = 0.5f;

        [field: SerializeField] public GameObject Prefab { get; private set; }
    }
}