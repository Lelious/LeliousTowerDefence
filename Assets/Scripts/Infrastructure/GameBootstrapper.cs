using Infrastructure.StateMachine;
using Infrastructure.StateMachine.States;
using UnityEngine;
using Zenject;

public class GameBootstrapper : MonoBehaviour
{
    private GameLoopStateMachine _gameLoopStateMachine;

    private void Start() =>
            _gameLoopStateMachine.Enter<GameLoadState>();

    [Inject]
    private void Construct(GameLoopStateMachine gameLoopStateMachine) =>
            _gameLoopStateMachine = gameLoopStateMachine;
}
