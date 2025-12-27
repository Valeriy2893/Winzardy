namespace _Project.Scripts.Infrastructure.EventBus
{
    public interface IEventListener<T> where T : struct
    {
        public void OnEvent(in T evt);
    }
}