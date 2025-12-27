using _Project.Scripts.Core.ECS.Components;

namespace _Project.Scripts.Features.Condition
{
    public struct TimerCondition
    {
        public bool IsMet(float dt, ref Timer timer)
        {
            timer.TimeLeft -= dt;
            if (timer.TimeLeft > 0f)
                return false;

            timer.TimeLeft = timer.Interval;
            return true;
        }
    }
}