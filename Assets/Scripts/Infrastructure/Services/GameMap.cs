using BezierSolution;
using Cysharp.Threading.Tasks;
using Infrastructure.StateMachine;
using System.Threading;
using UnityEngine;

public abstract class GameMap : MonoBehaviour
{
    [SerializeField] protected MapCameraLimits _limits;

    protected TopMenuInformator _topInformator;
    protected CancellationTokenSource _closeSpawnToken;
    protected GameMapColorScheme _colorScheme;
    protected EnemyFactory _enemyFactory;
    protected GameLoopStateMachine _gameLoopStateMachine;
    protected PointDescriptionData _pointDescriptionData;
    protected bool _isForceSpawn;
    public MapCameraLimits GetCameraLimits() => _limits;
    public abstract void StartSpawnPattern();
    public abstract void ForceSpawn();
    public abstract void ConstructGameMap(GameLoopStateMachine stateMachine, EnemyFactory enemyFactory, TopMenuInformator topInformator, PointDescriptionData pointDescriptionData, SelectedFrame selectedFrame, GameUIService gameUIService, TowerFactory towerFactory, PoolService poolService);
    protected virtual async UniTask EnemySpawn(CancellationTokenSource tokenSource) { }
    protected virtual Color GetRandomColor(Color color1, Color color2) => new Color(Random.Range(color1.r, color2.r),
                                                                                    Random.Range(color1.g, color2.g),
                                                                                    Random.Range(color1.b, color2.b));

    private void Awake()
    {
        _closeSpawnToken = new();
    }

    private void OnDestroy()
    {
        if(_closeSpawnToken != null)
        {
            _closeSpawnToken.Cancel();
            _closeSpawnToken.Dispose();
        }
    }
}