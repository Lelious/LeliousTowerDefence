using Zenject;

namespace Infrastructure.StateMachine.States
{
    public class GameLoadState : State
    {
        private GameLoopStateMachine _gameLoopStateMachine;
        private TowerFactory _towerFactory;
        private PoolService _poolService;
        private EnemyPool _enemyPool;

        public GameLoadState(GameLoopStateMachine gameLoopStateMachine) : base(gameLoopStateMachine) { _gameLoopStateMachine = gameLoopStateMachine; }

        [Inject]
        private void Construct(EnemyPool enemyPool, TowerFactory towerFactory, PoolService poolService)
        {
            _enemyPool = enemyPool;
            _poolService = poolService;
            _towerFactory = towerFactory;
        }

        public override void Enter()
        {
            _enemyPool.ClearEnemyPool();
            _poolService.RemoveAllBulletsFromPool();
            _towerFactory.ClearTowers();
        }

        public override void Exit()
        {
            
        }

    }
}
