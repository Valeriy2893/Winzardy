using _Project.Scripts.Core.ECS;

namespace _Project.Scripts.Features.Condition
{
    public readonly struct LimitCondition<TTag> where TTag : struct
    {
        private readonly int _limit;
        private readonly EcsFilter<TTag> _filter;

        public LimitCondition(int limit, EcsFilter<TTag> filter)
        {
            _limit = limit;
            _filter = filter;
        }

        public bool IsMet()
        {
            return _filter.Entities.Count < _limit;
        }
    }
}