using UnityEngine;
using Zenject;
using DG.Tweening;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Cysharp.Threading.Tasks;
using Infrastructure.StateMachine;
using Infrastructure.StateMachine.States;
using System.Threading;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform _startPoint, _endPoint;
    [SerializeField] private TopMenuInformator _topMenuInformator;
    [SerializeField] private CanvasGroup _fadeCanvas;
    [SerializeField] private AssetReferenceGameObject _miniMapAsset;
    [SerializeField] private AssetReferenceGameObject _gameMapAsset;

    private SelectedFrame _selectedFrame;
    private GameUIService _gameUIService;
    private TowerFactory _towerFactory;
    private PoolService _poolService;
    private EnemyFactory _enemyFactory;
    private ParentedCamera _camera;
    private GameLoopStateMachine _gameLoopStateMachine;
    private UniTask _fadeHandle;
    private AsyncOperationHandle<GameObject> _miniMapHandle;
    private AsyncOperationHandle<GameObject> _gameMapHandle;

    private GameMap _gameMap;
    private MiniMap _miniMap;

    private int _playerGold;

    private void OnEnable()
    {
        SwitchToMiniMap(true).Forget();
    }

    [Inject]
    private void Construct(TopMenuInformator topMenuInformator, GameLoopStateMachine stateMachine, ParentedCamera camera, EnemyFactory enemyFactory, SelectedFrame selectedFrame, GameUIService gameUIService, TowerFactory towerFactory, PoolService poolService)
    {
        _topMenuInformator = topMenuInformator;
        _camera = camera;
        _gameLoopStateMachine = stateMachine;
        _enemyFactory = enemyFactory;
        _selectedFrame = selectedFrame;
        _gameUIService = gameUIService;
        _towerFactory = towerFactory;
        //_topMenuInformator.SetMoney(_playerGold);
    }

    public async void TESTMoveOnMiniMap()
    {
        await SwitchToMiniMap();
    }

    public void ForceSpawn()
    {
        if(_gameMap != null)
        {
            _gameMap.ForceSpawn();
        }
    }

    public async UniTask SwitchToMiniMap(bool startInitializing = false)
    {
        if (_miniMap != null) return;

        _fadeHandle = _fadeCanvas.DOFade(1f, startInitializing == false ? 1f : 0f).AsyncWaitForCompletion().AsUniTask();
        await _fadeHandle.AsAsyncUnitUniTask();
        _miniMapHandle = _miniMapAsset.InstantiateAsync();
        await _miniMapHandle.ToUniTask();

        _miniMap = _miniMapHandle.Result.GetComponent<MiniMap>();
        _miniMap.InitializePath(_topMenuInformator.GetInfoField());
        _camera.SetNewLimits(_miniMap.GetCameraLimits());
        _topMenuInformator.SwitchToMiniMapInformator();
        if (_gameMapHandle.IsValid() && _gameMapHandle.Status == AsyncOperationStatus.Succeeded)
            Addressables.ReleaseInstance(_gameMapHandle);

        await UniTask.WaitUntil(() => _fadeHandle.Status.IsCompleted() == true);
        _fadeHandle = _fadeCanvas.DOFade(0f, 1f).AsyncWaitForCompletion().AsUniTask();
        _gameLoopStateMachine.Enter<GameLoadState>();
    }

    public async UniTask SwitchToGameMap(PointDescriptionData data)
    {
        if (_gameMap != null) return;

        _fadeHandle = _fadeCanvas.DOFade(1f, 1f).AsyncWaitForCompletion().AsUniTask();
        await _fadeHandle.AsAsyncUnitUniTask();
        _gameMapHandle = Addressables.InstantiateAsync(data.GameMapName);
        await _gameMapHandle.ToUniTask();

        _gameMap = _gameMapHandle.Result.GetComponent<GameMap>();
        _gameMap.ConstructGameMap(_gameLoopStateMachine, _enemyFactory, _topMenuInformator, data, _selectedFrame, _gameUIService, _towerFactory, _poolService);
        _camera.SetNewLimits(_gameMap.GetCameraLimits());
        _topMenuInformator.SwitchToGameInformator(data);
        _playerGold = data.StartGold;

        if (_miniMapHandle.IsValid() && _miniMapHandle.Status == AsyncOperationStatus.Succeeded)
            Addressables.ReleaseInstance(_miniMapHandle);

        await UniTask.WaitUntil(() => _fadeHandle.Status.IsCompleted() == true);
        _fadeHandle = _fadeCanvas.DOFade(0f, 1f).AsyncWaitForCompletion().AsUniTask();
        _gameLoopStateMachine.Enter<GameBuildingState>();
        _gameMap.StartSpawnPattern();
    }

    public bool CheckForGoldAvalability(int cost)
    {
        if (cost > _playerGold)
        {
            NotEnoughGold();
            return false;
        }
        else
        {
            _playerGold -= cost;
            _topMenuInformator.SetMoney(_playerGold);
            return true;           
        }
    }

    public void CompletePathPoint()
    {
        if (_miniMap == false) return;
        _miniMap.CompletePath();
    }
    public void ClearPath()
    {
        if (_miniMap == false) return;
        _miniMap.ClearPath();
    }

    public void AddGold(int amount)
    {
        _playerGold += amount;
        _topMenuInformator.SetMoney(_playerGold);
    }   

    private void NotEnoughGold()
    {

    }
}
