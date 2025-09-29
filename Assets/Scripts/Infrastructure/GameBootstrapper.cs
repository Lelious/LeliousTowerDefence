using Infrastructure.StateMachine;
using Infrastructure.StateMachine.States;
using UnityEngine;
using Zenject;

public class GameBootstrapper : IInitializable
{
    private GameLoopStateMachine _gameLoopStateMachine;

    [Inject]
    private void Construct(GameLoopStateMachine gameLoopStateMachine) =>
            _gameLoopStateMachine = gameLoopStateMachine;

    public void Initialize()
    {
        _gameLoopStateMachine.Enter<GameLoadState>();
    }
}
