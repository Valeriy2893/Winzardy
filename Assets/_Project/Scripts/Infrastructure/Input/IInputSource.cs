namespace _Project.Scripts.Infrastructure.Input
{
    public interface IInputSource<out T> where T : struct
    {
        public T Read();
    }
}