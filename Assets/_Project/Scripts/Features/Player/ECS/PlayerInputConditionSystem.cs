using _Project.Scripts.Core.ECS;
using _Project.Scripts.Core.ECS.Components;
using _Project.Scripts.Core.World;
using _Project.Scripts.Features.Condition;
using _Project.Scripts.Infrastructure.Input;

namespace _Project.Scripts.Features.Player.ECS
{
    public sealed class PlayerInputConditionSystem<TSource, TConditions> : ISystem
        where TSource : struct, IInputSource<InputData>
        where TConditions : struct, IEntityCondition
    {
        private TSource _source;
        private TConditions _conditions;
        private readonly EcsFilter<PlayerTag, Direction> _filter;

        public PlayerInputConditionSystem(TSource source, TConditions conditions,
            EcsFilter<PlayerTag, Direction> filter)
        {
            _source = source;
            _conditions = conditions;
            _filter = filter;
        }

        public void Update(World world, float dt)
        {
            if (_filter.Entities.Count == 0)
                return;

            for (int i = 0; i < _filter.Entities.Count; i++)
            {
                if (!_conditions.IsMet(world, dt, _filter.Entities[i]))
                    continue;

                ref var dir = ref world.GetPool<Direction>().Get(_filter.Entities[i]);

                var input = _source.Read();
                dir.X = input.X;
                dir.Z = input.Z;
            }
        }
    }
}