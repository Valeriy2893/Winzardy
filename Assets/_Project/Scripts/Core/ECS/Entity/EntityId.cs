using System;

namespace _Project.Scripts.Core.ECS.Entity
{
    public readonly struct EntityId : IEquatable<EntityId>
    {
        public readonly int Value;

        public EntityId(int value)
        {
            Value = value;
        }

        public bool Equals(EntityId other)
        {
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            return obj is EntityId other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Value;
        }

        public static bool operator ==(EntityId a, EntityId b)
        {
            return a.Value == b.Value;
        }

        public static bool operator !=(EntityId a, EntityId b)
        {
            return a.Value != b.Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}