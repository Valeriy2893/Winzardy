using _Project.Scripts.Core.ECS;
using _Project.Scripts.Core.ECS.Components;

namespace _Project.Scripts.Features.Condition
{
    public readonly struct ViewportCondition
    {
        private readonly EcsFilter<ViewportBounds> _viewports;

        public ViewportCondition(EcsFilter<ViewportBounds> viewports)
        {
            _viewports = viewports;
        }

        public bool IsMet()
        {
            return _viewports.Entities.Count > 0;
        }
    }
}