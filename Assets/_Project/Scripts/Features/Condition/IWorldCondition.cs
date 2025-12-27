using _Project.Scripts.Core.World;

namespace _Project.Scripts.Features.Condition
{
    public interface IWorldCondition
    {
        public bool IsMet(World world, float dt);
    }
}