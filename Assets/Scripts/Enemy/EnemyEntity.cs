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

    private GameInformationMenu _gameInformationMenu;
    private BottomMenuInformator _bottomMenuInformator;
    private SelectedFrame _selectedFrame;
    private GameManager _gameManager;
    private EnemyPool _enemyPool;
    private bool _isTouched;

  
    [Inject]
    private void Construct(EnemyPool enemyPool, GameManager gameManager, GameInformationMenu gameInformationMenu, SelectedFrame selectedFrame)
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
    }

    public void ReturnToEnemyPool()
    {
        _enemyPool.ReturnToPool(this);
        _gameManager.AddGold(_enemyData.Worth);

        StartCoroutine(DelayedDisableRoutine());
    }

    private IEnumerator DelayedDisableRoutine()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }

    public void Touch()
    {
        _isTouched = true;
        _gameInformationMenu.ShowGameMenu();
        _bottomMenuInformator.SetEntityToPannelUpdate(this, _enemyData.MainImage, Health, _enemyData.Name, _enemyData.Hp, 0, 0, _enemyData.Armor, 0);
        _selectedFrame.DisableFrame();
        _selection.SetActive(true);
    }

    public void Untouch()
    {
        _isTouched = false;
        _selection.SetActive(false);
    }

    public Vector3 GetPosition()
    {
        throw new System.NotImplementedException();
    }

    public bool IsTouched() => _isTouched;
}
