using Infrastructure.StateMachine.States;
using System;
using System.Collections.Generic;
using Zenject;

namespace Infrastructure.StateMachine
{
    public class GameLoopStateMachine : IInitializable
    {
        private readonly DiContainer _diContainer;
        private Dictionary<Type, IState> _states;
        private IState _activeState;

        public GameLoopStateMachine(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }
        public IState ActiveState => _activeState;

        public void Initialize()
        {
            _states = new Dictionary<Type, IState>
        {
            { typeof(GameLoadState), new GameLoadState(this) },         
            { typeof(GameState), new GameState(this) },
            { typeof(GameWinState), new GameWinState(this) },
            { typeof(GameLooseState), new GameLooseState(this) },
            { typeof(GameResetState), new GameResetState(this) },
            { typeof(GameSpawnState), new GameSpawnState(this) },
            { typeof (GameBuildingState), new GameBuildingState(this)}
        };
            _states.ForEach(x => _diContainer.Inject(x.Value));
        }

        public void Enter<TState>() where TState : class, IState
        {
            IState state = ChangeState<TState>();
            state.Enter();
        }

        private TState ChangeState<TState>() where TState : class, IState
        {
            _activeState?.Exit();

            TState state = GetState<TState>();
            _activeState = state;

            return state;
        }

        private TState GetState<TState>() where TState : class, IState =>
          _states[typeof(TState)] as TState;
    }
}
