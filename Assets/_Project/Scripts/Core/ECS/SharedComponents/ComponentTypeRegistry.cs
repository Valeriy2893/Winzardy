using System;

namespace _Project.Scripts.Core.ECS.SharedComponents
{
    public sealed class ComponentTypeRegistry
    {
        private int _nextId;
        private int[] _ids = new int[32]; 

        public int GetId<T>() where T : struct
        {
            int index = ComponentTypeIndex<T>.Index;

            if (index >= _ids.Length)
                Array.Resize(ref _ids, index * 2);

            int stored = _ids[index];
            if (stored != 0)
                return stored - 1;

            int id = _nextId++;
            _ids[index] = id + 1;
            return id;
        }

        public void Clear()
        {
            _nextId = 0;
            Array.Clear(_ids, 0, _ids.Length);
        }

        private static int _globalTypeIndex;

        private static class ComponentTypeIndex<T>
        {
            public static readonly int Index = _globalTypeIndex++;
        }
    }
}