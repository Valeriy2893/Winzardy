using _Project.Scripts.Core.ECS;
using _Project.Scripts.Core.World;
using _Project.Scripts.Features.Action;
using _Project.Scripts.Features.Condition;

namespace _Project.Scripts.Features.StateChage
{
    public sealed class StateChangeSystem<TAction, TCondition, TFilter> : ISystem
        where TAction : struct, IAction
        where TCondition : struct, IEntityCondition
        where TFilter : struct
    {
        private TAction _action;
        private TCondition _condition;
        private readonly EcsFilter<TFilter> _filter;

        public StateChangeSystem(TAction action, TCondition condition, EcsFilter<TFilter> filter)
        {
            _action = action;
            _condition = condition;
            _filter = filter;
        }

        public void Update(World world, float dt)
        {
            var entities = _filter.Entities;

            for (int i = 0; i < entities.Count; i++)
            {
                if (!_condition.IsMet(world, dt, entities[i])) continue;
                _action.Execute(world, entities[i], dt);
            }
        }
    }
}