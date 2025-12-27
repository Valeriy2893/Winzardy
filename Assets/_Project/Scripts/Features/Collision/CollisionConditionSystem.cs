using _Project.Scripts.Core.ECS;
using _Project.Scripts.Core.ECS.Components;
using _Project.Scripts.Core.World;
using _Project.Scripts.Features.Action;
using _Project.Scripts.Features.Condition;

namespace _Project.Scripts.Features.Collision
{
    public sealed class CollisionConditionSystem<TTag, TAction, TCondition> : ISystem
        where TTag : struct
        where TAction : struct, IAction
        where TCondition : struct, IEntityCondition
    {
        private TAction _action;
        private TCondition _condition;
        private readonly EcsFilter<TTag, Position, CollisionRadius> _filter;

        public CollisionConditionSystem(TAction action, TCondition condition,
            EcsFilter<TTag, Position, CollisionRadius> filter)
        {
            _action = action;
            _condition = condition;
            _filter = filter;
        }

        public void Update(World world, float dt)
        {
            if (_filter.Entities.Count == 0)
                return;

            for (int i = 0; i < _filter.Entities.Count; i++)
            {
                if (!_condition.IsMet(world, dt, _filter.Entities[i]))
                    continue;

                _action.Execute(world, _filter.Entities[i], dt);
            }
        }
    }
}