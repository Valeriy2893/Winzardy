using _Project.Scripts.Core.ECS.Components;

namespace _Project.Scripts.Infrastructure.Input.KeyboardInput
{
    public readonly struct KeyboardInputSource : IInputSource<InputData>
    {
        private readonly KeyboardInputAdapter _adapter;

        public KeyboardInputSource(KeyboardInputAdapter adapter)
        {
            _adapter = adapter;
        }

        public InputData Read()
        {
            return _adapter.Current;
        }
    }
}