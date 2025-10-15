using BezierSolution;
using Cysharp.Threading.Tasks;
using Infrastructure.StateMachine;
using System.Threading;
using UnityEngine;

public class AlchemyMap : GameMap
{
    [SerializeField] private WizardController _wizardController;
    [SerializeField] private JarEffectService _effectService;
    [SerializeField] private BezierSpline _leftPath, _rightPath;
    [SerializeField] private BuildingGround _buildGround;

    private BezierWalkerWithTime _walker;
    private bool _canEmitNexyEnemy;

    public override void ConstructGameMap(GameLoopStateMachine stateMachine, EnemyFactory enemyFactory, TopMenuInformator topInformator, PointDescriptionData pointDescriptionData, SelectedFrame selectedFrame, GameUIService gameUIService, TowerFactory towerFactory, PoolService poolService)
    {
        _topInformator = topInformator;
        _gameLoopStateMachine = stateMachine;
        _enemyFactory = enemyFactory;
        _pointDescriptionData = pointDescriptionData;
        _walker = _effectService.GetEffectWalker();
        _walker.onPathCompleted.AddListener(EmitEnemy);
    }

    public override void ForceSpawn()
    {
        _isForceSpawn = true;
    }

    public override void StartSpawnPattern()
    {
        EnemySpawn(_closeSpawnToken).Forget();
    }

    protected override async UniTask EnemySpawn(CancellationTokenSource token)
    {
        var firstDelay = _pointDescriptionData.SpawnScheme.SpawnPattern[0].Delay;

        _enemyFactory.CreateEnemy(_pointDescriptionData.SpawnScheme).Forget();
        _topInformator.EnableCounter();

        while (firstDelay > 0 && _isForceSpawn == false)
        {
            _topInformator.SetSpawnTime((int)firstDelay);
            await UniTask.WaitForSeconds(1f).SuppressCancellationThrow();
            firstDelay -= 1f;
        }

        _topInformator.EnterSpawnState();
        _topInformator.DisableCounter();

        for (int i = 1; i < _pointDescriptionData.SpawnScheme.SpawnPattern.Count; i++)
        {
            if (!token.IsCancellationRequested)
            {
                switch (_pointDescriptionData.SpawnScheme.SpawnPattern[i].SpawnPhase)
                {
                    case SpawnPattern.Spawn:
                        var nextEnemy = _enemyFactory.GetEnemy();
                        var rnd = Random.Range(0, 2);
                        _effectService.EmitWalker(rnd);
                        nextEnemy.SetMovePath(rnd == 0 ? _leftPath : _rightPath);
                        await UniTask.WaitUntil(() => _canEmitNexyEnemy == true);

                        if (!token.IsCancellationRequested)
                        {
                            nextEnemy.gameObject.SetActive(true);
                            _canEmitNexyEnemy = false;
                        }

                        await UniTask.WaitForSeconds(_pointDescriptionData.SpawnScheme.SpawnPattern[i].Delay);

                        break;

                    case SpawnPattern.Wait:
                        await UniTask.WaitForSeconds(_pointDescriptionData.SpawnScheme.SpawnPattern[i].Delay);
                        break;
                }
            }
        }
    }

    private void EmitEnemy()
    {
        Debug.Log("PathCompleted");
        _canEmitNexyEnemy = true;
    }
}
