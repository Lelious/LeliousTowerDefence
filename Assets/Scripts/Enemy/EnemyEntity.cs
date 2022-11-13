using System;
using System.Collections;
using UniRx;
using UnityEngine;
using Zenject;

public class EnemyEntity : MonoBehaviour, ITouchable
{
    public FloatReactiveProperty Health = new FloatReactiveProperty();

    [SerializeField] private EnemyHealth _enemyHealth;
    [SerializeField] private EnemyData _enemyData;
    [SerializeField] private GameObject _selection;

    private GamePannelUdaterInfoContainer _containerInfo;
    private GameUIService _gameInformationMenu;
    private BottomGameMenu _bottomMenuInformator;
    private SelectedFrame _selectedFrame;
    private GameManager _gameManager;
    private EnemyPool _enemyPool;
    private bool _isTouched;

  
    [Inject]
    private void Construct(EnemyPool enemyPool, GameManager gameManager, GameUIService gameInformationMenu, SelectedFrame selectedFrame)
    {
        _enemyPool = enemyPool;
        _gameManager = gameManager;
        _gameInformationMenu = gameInformationMenu;
        _bottomMenuInformator = _gameInformationMenu.GetBottomMenuInformator();
        _selectedFrame = selectedFrame;       
    }

    private void Awake()
    {
        Health = _enemyHealth.GetReactiveHealthProperty();
        InitializeInfoContainer();
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
        _selection.SetActive(true);
    }

    public void Untouch()
    {
        _isTouched = false;
        _selection.SetActive(false);
    }

    public Vector3 GetPosition() => transform.position;
    public bool IsTouched() => _isTouched;

    private IEnumerator DelayedDisableRoutine()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }

    private void InitializeInfoContainer()
    {
        _containerInfo = new GamePannelUdaterInfoContainer();

        _containerInfo.Touchable = this;
        _containerInfo.PreviewImage = _enemyData.MainImage;
        _containerInfo.CurrentHealth = Health;
        _containerInfo.Name = _enemyData.Name;
        _containerInfo.MaxHealth = _enemyData.Hp;
        _containerInfo.MinDamage = 0;
        _containerInfo.MaxDamage = 0;
        _containerInfo.Armor = _enemyData.Armor;
        _containerInfo.AttackSpeed = 0;
    }
}
