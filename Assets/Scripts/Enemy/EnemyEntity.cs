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
        gameObject.layer = 2;
    }

    private void InitializeInfoContainer()
    {
        _containerInfo = new GamePannelUdaterInfoContainer
        {
            Touchable = this,
            PreviewImage = _enemyData.MainImage,
            CurrentHealth = Health,
            Name = _enemyData.Name,
            MaxHealth = _enemyData.Hp,
            MinDamage = 0,
            MaxDamage = 0,
            Armor = _enemyData.Armor,
            AttackSpeed = 0,
            UpgradesList = null
        };
    }

    public TouchableType GetTouchableType() => TouchableType.Enemy;
}
