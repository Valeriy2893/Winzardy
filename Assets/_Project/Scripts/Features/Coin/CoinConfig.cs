using UnityEngine;

namespace _Project.Scripts.Features.Coin
{
    [CreateAssetMenu(menuName = "Configs/Coin Config")]
    public sealed class CoinConfig : ScriptableObject
    {
        [field: SerializeField] public float CollisionRadius { get; private set; } = 0.5f;
        [field: SerializeField] public GameObject Prefab { get; private set; }
    }
}