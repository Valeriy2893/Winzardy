using _Project.Scripts.Core.ECS;
using _Project.Scripts.Core.ECS.Components;
using _Project.Scripts.Features.Action;
using _Project.Scripts.Features.Coin;
using _Project.Scripts.Features.Collision;
using _Project.Scripts.Features.Collision.Coin;
using _Project.Scripts.Features.Collision.Enemy;
using _Project.Scripts.Features.Collision.Projectile;
using _Project.Scripts.Features.Condition;
using _Project.Scripts.Features.Enemy;
using _Project.Scripts.Features.EnemySpawner;
using _Project.Scripts.Features.GameOver.UI;
using _Project.Scripts.Features.Movement;
using _Project.Scripts.Features.Movement.Enemy;
using _Project.Scripts.Features.Movement.Player;
using _Project.Scripts.Features.Movement.Projectile;
using _Project.Scripts.Features.Player;
using _Project.Scripts.Features.Player.ECS;
using _Project.Scripts.Features.Player.UI;
using _Project.Scripts.Features.Projectile;
using _Project.Scripts.Features.Spawn;
using _Project.Scripts.Features.Spawn.Coin;
using _Project.Scripts.Features.Spawn.Enemy;
using _Project.Scripts.Features.Spawn.EnemySpawner;
using _Project.Scripts.Features.Spawn.Player;
using _Project.Scripts.Features.Spawn.Projectile;
using _Project.Scripts.Features.StateChage;
using _Project.Scripts.Infrastructure.Input.KeyboardInput;
using _Project.Scripts.Infrastructure.View;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = System.Random;

namespace _Project.Scripts.Core.Bootstrap
{
    public sealed class GameEntryPoint : MonoBehaviour
    {
        [SerializeField] private PlayerResourceUI _playerResourceUI;
        [SerializeField] private PlayerHealthUI _playerHealthUI;

        [SerializeField] private PlayerConfig _playerConfig;
        [SerializeField] private EnemyConfig _enemyConfig;
        [SerializeField] private CoinConfig _coinConfig;
        [SerializeField] private ProjectileConfig _projectileConfig;
        [SerializeField] private EnemySpawnerConfig _enemySpawnerConfig;

        [SerializeField] private Transform _poolRoot;
        [SerializeField] private KeyboardInputAdapter _keyboardInput;
        [SerializeField] private GameObject _panel;
        [SerializeField] private Button _buttonRestart;

        private World.World _world;
        private ISystem[] _systems;

        private ViewPool _projectilePool;
        private ViewPool _enemyPool;
        private ViewPool _coinPool;
        private ViewRegistry _viewRegistry;

        //Debug
        public World.World World => _world;
        public ViewRegistry ViewRegistry => _viewRegistry;

        private void Start()
        {
            _world = new World.World();
            CreateView();
            CreateGameplaySystems();
            InitCoinSpawnExecuteSystem();
        }

        private void Update()
        {
            foreach (var system in _systems)
                system.Update(_world, Time.deltaTime);
        }

        private void OnDestroy()
        {
            _world.Dispose();
        }

        private void CreateView()
        {
            CreateViewPools();
            CreateViewRegistry();
            RegisterSubscriptions();
            CreateUI();
        }

        private void CreateGameplaySystems()
        {
            _systems = new[]
            {
                CreatePlayerSpawnConditionSystem(),
                CreateEnemySpawnerConditionSystem(),
                CreateEnemySpawnConditionSystem(),
                CreatePlayerProjectileSpawnConditionSystem(),
                CreatePlayerInputConditionSystem(),
                CreatePlayerMovementSystem(),
                CreateEnemyMovementSystem(),
                CreateProjectileMovementSystem(),
                CreateCoinPickupCollisionConditionSystem(),
                CreateProjectileCollisionConditionSystem(),
                CreateEnemyAttackCollisionConditionSystem(),
                CreateDeathSystem(),
                CreateLifetimeSystem(),
            };
        }

        private void InitCoinSpawnExecuteSystem()
        {
            var factory = new CoinSpawnFactory(_coinConfig);
            var executeSystem = new SpawnExecuteSystem<CoinTag, CoinSpawnFactory>(_world, factory);
            _world.EventBus.CoinSpawnRequests.Subscribe(executeSystem);
        }

        private ISystem CreateEnemyAttackCollisionConditionSystem()
        {
            var enemyAttackConditions = new EnemyAttackConditions(_world.GetFilter<PlayerTag, Position>());
            var enemyAttackAction = new EnemyAttackAction(_world.GetFilter<PlayerTag, Health>());
            return new CollisionConditionSystem<EnemyTag, EnemyAttackAction, EnemyAttackConditions>(
                enemyAttackAction, enemyAttackConditions, _world.GetFilter<EnemyTag, Position, CollisionRadius>());
        }

        private ISystem CreateCoinPickupCollisionConditionSystem()
        {
            var coinPickupConditions =
                new CoinPickupConditions(_world.GetFilter<PlayerTag, Position, CollisionRadius>());
            var coinPickupAction = new CoinPickupAction(_world.GetFilter<PlayerTag, Resource>());
            return new CollisionConditionSystem<CoinTag, CoinPickupAction, CoinPickupConditions>(
                coinPickupAction, coinPickupConditions, _world.GetFilter<CoinTag, Position, CollisionRadius>());
        }

        private ISystem CreateProjectileCollisionConditionSystem()
        {
            var projectileCollisionConditions =
                new ProjectileCollisionConditions(_world.GetFilter<EnemyTag, Position, CollisionRadius>());
            var projectileCollisionAction =
                new PlayerProjectileHitAction(_world.GetFilter<EnemyTag, Position, CollisionRadius, Health>());
            return new
                CollisionConditionSystem<ProjectileTag, PlayerProjectileHitAction, ProjectileCollisionConditions>(
                    projectileCollisionAction, projectileCollisionConditions,
                    _world.GetFilter<ProjectileTag, Position, CollisionRadius>());
        }

        private ISystem CreatePlayerInputConditionSystem()
        {
            var inputSource = new KeyboardInputSource(_keyboardInput);
            var playerInputCondition = new PlayerInputConditions(new AliveCondition());
            return new PlayerInputConditionSystem<KeyboardInputSource, PlayerInputConditions>(inputSource,
                playerInputCondition, _world.GetFilter<PlayerTag, Direction>());
        }

        private ISystem CreateDeathSystem()
        {
            return new StateChangeSystem<DeathAction, DeathCondition, Health>(new DeathAction(),
                new DeathCondition(), _world.GetFilter<Health>());
        }

        private ISystem CreatePlayerMovementSystem()
        {
            var playerMovementConditions = new PlayerMovementConditions(new AliveCondition(), new DirectionCondition());
            var playerActions = new PlayerMoveAction();
            return new MovementConditionSystem<PlayerTag, PlayerMoveAction, PlayerMovementConditions>(
                playerActions, playerMovementConditions, _world.GetFilter<PlayerTag, Position, Velocity, Direction>());
        }

        private ISystem CreateProjectileMovementSystem()
        {
            var projectileMovementConditions = new ProjectileMovementConditions(new DirectionCondition());
            var projectileMoveAction = new ProjectileMoveAction();
            return new MovementConditionSystem<ProjectileTag, ProjectileMoveAction, ProjectileMovementConditions>(
                projectileMoveAction, projectileMovementConditions,
                _world.GetFilter<ProjectileTag, Position, Velocity, Direction>());
        }

        private ISystem CreateEnemyMovementSystem()
        {
            var enemyMoveConditions = new EnemyMovementConditions(new AliveCondition(), new DirectionCondition());
            var enemyMoveAction = new EnemyMoveAction(_world.GetFilter<PlayerTag, Position>());
            return new MovementConditionSystem<EnemyTag, EnemyMoveAction, EnemyMovementConditions>(
                enemyMoveAction, enemyMoveConditions, _world.GetFilter<EnemyTag, Position, Velocity, Direction>());
        }

        private ISystem CreateLifetimeSystem()
        {
            return new StateChangeSystem<DeathAction, LifetimeExpiredCondition, Lifetime>(
                new DeathAction(), new LifetimeExpiredCondition(), _world.GetFilter<Lifetime>());
        }

        private void CreateViewPools()
        {
            _projectilePool = new ViewPool(_projectileConfig.Prefab, _poolRoot);
            _enemyPool = new ViewPool(_enemyConfig.Prefab, _poolRoot);
            _coinPool = new ViewPool(_coinConfig.Prefab, _poolRoot);
        }

        private void CreateViewRegistry()
        {
            _viewRegistry = new ViewRegistry(_projectilePool, _enemyPool, _coinPool, _world);
        }

        private void RegisterSubscriptions()
        {
            _world.EventBus.EntityCreated.Subscribe(new ViewSpawner<PlayerTag>(_world, _viewRegistry,
                _playerConfig.Prefab));
            _world.EventBus.EntityCreated.Subscribe(new ViewSpawner<EnemyTag>(_world, _viewRegistry, _enemyPool));
            _world.EventBus.EntityCreated.Subscribe(new ViewSpawner<CoinTag>(_world, _viewRegistry, _coinPool));
            _world.EventBus.EntityCreated.Subscribe(new ViewSpawner<ProjectileTag>(_world, _viewRegistry,
                _projectilePool));

            _world.EventBus.PositionChanged.Subscribe(new ViewPositionListener(_viewRegistry));

            _world.EventBus.EntityDestroyed.Subscribe(new ViewCleanupListener(_viewRegistry));
            _world.EventBus.EntityDestroyed.Subscribe(new GameOverUIListener(_world, _panel, _buttonRestart,
                () => SceneManager.LoadScene(0)));
            _world.EventBus.EntityDestroyed.Subscribe(new CoinDropListener(_world, _enemyConfig, new Random()));
        }

        private void CreateUI()
        {
            _playerHealthUI.Init(_world);
            _playerResourceUI.Init(_world);
        }

        private ISystem CreatePlayerSpawnConditionSystem()
        {
            var factory = new PlayerSpawnFactory(_playerConfig);
            var executeSystem = new SpawnExecuteSystem<PlayerTag, PlayerSpawnFactory>(_world, factory);

            _world.EventBus.PlayerSpawnRequests.Subscribe(executeSystem);

            var conditions = new PlayerSpawnConditions(
                new LimitCondition<PlayerTag>(1, _world.GetFilter<PlayerTag>())
            );

            var builder = new PlayerSpawnRequestBuilder();

            return new SpawnConditionSystem<PlayerTag, PlayerSpawnConditions, PlayerSpawnRequestBuilder>
            (
                _world.EventBus.PlayerSpawnRequests,
                conditions,
                builder
            );
        }

        private ISystem CreateEnemySpawnerConditionSystem()
        {
            var factory = new EnemySpawnerFactory(_enemySpawnerConfig);
            var executeSystem = new SpawnExecuteSystem<EnemySpawnerTag, EnemySpawnerFactory>(_world, factory);

            _world.EventBus.EnemySpawnerRequests.Subscribe(executeSystem);

            var conditions =
                new EnemySpawnerConditions(new LimitCondition<EnemySpawnerTag>(1, _world.GetFilter<EnemySpawnerTag>()));

            var builder = new EnemySpawnerRequestBuilder(_enemySpawnerConfig, new Random());
            return new SpawnConditionSystem<EnemySpawnerTag, EnemySpawnerConditions, EnemySpawnerRequestBuilder>
            (
                _world.EventBus.EnemySpawnerRequests, conditions, builder
            );
        }

        private ISystem CreateEnemySpawnConditionSystem()
        {
            var factory = new EnemySpawnFactory(_enemyConfig);
            var executeSystem =
                new SpawnExecuteSystem<EnemyTag, EnemySpawnFactory>(_world, factory);

            _world.EventBus.EnemySpawnRequests.Subscribe(executeSystem);

            var conditions = new EnemySpawnConditions(_world.GetFilter<EnemySpawnerTag>(), new TimerCondition(),
                new ViewportCondition(_world.GetFilter<ViewportBounds>()));

            var builder = new EnemySpawnRequestBuilder(_enemyConfig, new Random());
            return new SpawnConditionSystem<EnemyTag, EnemySpawnConditions, EnemySpawnRequestBuilder>
            (
                _world.EventBus.EnemySpawnRequests,
                conditions,
                builder
            );
        }

        private ISystem CreatePlayerProjectileSpawnConditionSystem()
        {
            var factory = new PlayerProjectileSpawnFactory(_playerConfig);
            var executeSystem =
                new SpawnExecuteSystem<ProjectileTag, PlayerProjectileSpawnFactory>(_world, factory);

            _world.EventBus.ProjectileSpawnRequests.Subscribe(executeSystem);

            var conditions = new PlayerProjectileSpawnConditions(
                new AliveCondition(),
                new TimerCondition(), _world.GetFilter<PlayerTag>()
            );

            var builder = new PlayerProjectileSpawnRequestBuilder(_world.GetFilter<PlayerTag, Position>());

            return new SpawnConditionSystem<ProjectileTag, PlayerProjectileSpawnConditions,
                PlayerProjectileSpawnRequestBuilder>(
                _world.EventBus.ProjectileSpawnRequests,
                conditions,
                builder
            );
        }
    }
}