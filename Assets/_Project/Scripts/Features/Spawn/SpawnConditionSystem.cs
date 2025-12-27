using _Project.Scripts.Core.ECS;
using _Project.Scripts.Core.World;
using _Project.Scripts.Features.Condition;
using _Project.Scripts.Infrastructure.EventBus;

namespace _Project.Scripts.Features.Spawn
{
    public sealed class SpawnConditionSystem<TTag, TConditions, TBuilder> : ISystem
        where TTag : struct
        where TConditions : struct, IWorldCondition
        where TBuilder : struct, ISpawnRequestBuilder<TTag>
    {
        private readonly EventChannel<SpawnRequest<TTag>> _requests;
        private TConditions _conditions;
        private TBuilder _builder;

        public SpawnConditionSystem(EventChannel<SpawnRequest<TTag>> requests, TConditions conditions, TBuilder builder)
        {
            _requests = requests;
            _conditions = conditions;
            _builder = builder;
        }

        public void Update(World world, float dt)
        { 
            if (!_conditions.IsMet(world, dt)) return;
            _requests.Raise(_builder.Build(world));
        }
    }
}