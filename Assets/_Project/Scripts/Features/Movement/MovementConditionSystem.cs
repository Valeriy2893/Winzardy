using _Project.Scripts.Core.ECS;
using _Project.Scripts.Core.ECS.Components;
using _Project.Scripts.Core.World;
using _Project.Scripts.Features.Action;
using _Project.Scripts.Features.Condition;

namespace _Project.Scripts.Features.Movement
{
    public sealed class MovementConditionSystem<TTag, TActions, TConditions> : ISystem
        where TTag : struct
        where TActions : struct, IAction
        where TConditions : struct, IEntityCondition
    {
        private TConditions _conditions;
        private TActions _actions;
        private readonly EcsFilter<TTag, Position, Velocity, Direction> _filter;

        public MovementConditionSystem(TActions actions, TConditions conditions,
            EcsFilter<TTag, Position, Velocity, Direction> filter)
        {
            _actions = actions;
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

                _actions.Execute(world, _filter.Entities[i], dt);
            }
        }
    }
}