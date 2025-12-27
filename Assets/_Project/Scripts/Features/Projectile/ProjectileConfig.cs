using UnityEngine;

namespace _Project.Scripts.Features.Projectile
{
    [CreateAssetMenu(menuName = "Configs/Projectile Config")]
    public class ProjectileConfig : ScriptableObject
    {
        [field: SerializeField] public GameObject Prefab { get; private set; }
    }
}