using UnityEngine;

namespace _Project.Scripts.Features.EnemySpawner
{
    [CreateAssetMenu(menuName = "Configs/EnemySpawner Config")]
    public sealed class EnemySpawnerConfig : ScriptableObject
    {
        [field: SerializeField] public float SpawnInterval { get; private set; } = 7f;
        [field: SerializeField] public float SpawnOffset { get; private set; } = 2f;
    }
}