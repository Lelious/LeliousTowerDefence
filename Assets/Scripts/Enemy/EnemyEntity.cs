using System;
using System.Collections;
using UniRx;
using UnityEngine;
using Zenject;

public class EnemyEntity : MonoBehaviour, ITouchable
{
    [SerializeField] private EnemyHealth _enemyHealth;
    [SerializeField] private EnemyData _enemyData;
    [SerializeField] private GameObject _selection;

    private EnemyStats _enemyStats;
    private GamePannelUdaterInfoContainer _containerInfo;
    private GameUIService _gameInformationMenu;
    private BottomGameMenu _bottomMenuInformator;
    private SelectedFrame _selectedFrame;
    private GameManager _gameManager;
    private EnemyPool _enemyPool;
    private bool _isTouched;

    GameObject ITouchable.gameObject { get => gameObject; }

    [Inject]
    private void Construct(EnemyPool enemyPool, GameManager gameManager, GameUIService gameInformationMenu, SelectedFrame selectedFrame)
    {
        _enemyPool = enemyPool;
        _gameManager = gameManager;
        _gameInformationMenu = gameInformationMenu;
        _bottomMenuInformator = _gameInformationMenu.GetBottomMenuInformator();
        _selectedFrame = selectedFrame;
    }

    public void InitializeEnemy()
    {
        _enemyStats = new EnemyStats();
        _enemyStats.InitializeStats(_enemyData);
        _enemyStats.InitializeInfoContainer();
        _containerInfo = _enemyStats.GetContainer();
        _enemyHealth.InitializeHealth(_enemyStats.MaxHealth, _enemyStats.Health);
    }

    public void ReturnToEnemyPool()
    {
        _enemyPool.ReturnToPool(this);
        _gameManager.AddGold(_enemyData.Worth);

        StartCoroutine(DelayedDisableRoutine());
    }

    public void Touch()
    {
        _isTouched = true;
        _gameInformationMenu.ShowGameMenu();
        _bottomMenuInformator.SetEntityToPannelUpdate(_containerInfo);
        _selectedFrame.DisableFrame();
        _selection.SetActive(_isTouched);
    }

    public void Untouch()
    {
        _isTouched = false;
        _selection.SetActive(_isTouched);
    }

    public Vector3 GetPosition() => transform.position;
    public bool IsTouched() => _isTouched;

    private IEnumerator DelayedDisableRoutine()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
        gameObject.layer = 2;
    }
}
