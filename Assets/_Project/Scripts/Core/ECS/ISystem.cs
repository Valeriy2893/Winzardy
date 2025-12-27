namespace _Project.Scripts.Core.ECS
{
    public interface ISystem
    {
        void Update(World.World world, float deltaTime);
    }
}