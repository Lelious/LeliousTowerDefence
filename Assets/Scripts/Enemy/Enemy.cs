using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Zenject;

public class Enemy : MonoBehaviour
{
	[SerializeField] private Sprite _mainImage;
	[SerializeField] private string _name;
	[SerializeField] private GameObject _selection;
	[SerializeField] private NavMeshAgent _navMeshAgent;
	[SerializeField] private Animator _animator;
	[SerializeField] private float _navMeshSpeedConst = 2f;

	private float _speed = 1f;
	private readonly int _hashSpeed = Animator.StringToHash("Speed");
	private BottomMenuInformator _menuUpdater;
	private BuildCellInitializer _buildCellChanger;
	private BottomBuildingMenu _gameBottomPanel;

	[Inject]
	private void Construct(BottomBuildingMenu bottomPanel, BuildCellInitializer cellChanger, BottomMenuInformator menuUpdater)
	{
		_gameBottomPanel = bottomPanel;
		_buildCellChanger = cellChanger;
		_menuUpdater = menuUpdater;
	}

	private protected void LateUpdate()
	{
		_animator.SetFloat(_hashSpeed, _speed);
		_navMeshAgent.speed = _navMeshSpeedConst * _speed;
	}	

	public void EnableSelectFrame()
	{
		_selection.SetActive(true);
	}

	public void DisableSelectFrame()
	{
		_selection.SetActive(false);
	}

	public void UpgradeInformation()
	{
		//_menuUpdater.UpgradeInformation(_mainImage, _name, 0, 0, _armor, 0, $"{_currentHealth}/{_health}", _fill.color);
	}

	public void SetPath(Vector3 targetToMove)
	{
		_navMeshAgent.SetDestination(targetToMove);
	}
}
