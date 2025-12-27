using UnityEngine;

namespace _Project.Scripts.Features.Player
{
    [CreateAssetMenu(menuName = "Configs/Player Config")]
    public sealed class PlayerConfig : ScriptableObject
    {
        [Header("Movement")] [field: SerializeField]
        public float MoveSpeed { get; private set; } = 7f;

        [Header("Health")] [field: SerializeField]
        public float MaxHealth { get; private set; } = 100f;

        [Header("Shooting")] [field: SerializeField]
        public float ShootInterval { get; private set; } = 1f;

        [field: SerializeField] public float ProjectileDamage { get; private set; } = 25f;
        [field: SerializeField] public float ProjectileSpeed { get; private set; } = 10f;
        [field: SerializeField] public float ProjectileRadius { get; private set; } = 0.2f;
        [field: SerializeField] public float CollisionRadius { get; private set; } = 0.5f;
        [field: SerializeField] public float ProjectileLifetime { get; private set; } = 5f;
        [field: SerializeField] public GameObject Prefab { get; private set; }
    }
}