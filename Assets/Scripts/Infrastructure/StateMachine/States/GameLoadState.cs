using Zenject;

namespace Infrastructure.StateMachine.States
{
    public class GameLoadState : State
    {
        private GameLoopStateMachine _gameLoopStateMachine;
        public GameLoadState(GameLoopStateMachine gameLoopStateMachine) : base(gameLoopStateMachine) { _gameLoopStateMachine = gameLoopStateMachine; }

        public override void Enter()
        {
            _gameLoopStateMachine.Enter<GameBuildingState>();
        }

        public override void Exit()
        {
            
        }

    }
}
