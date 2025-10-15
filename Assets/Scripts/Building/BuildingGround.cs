using System.Collections.Generic;
using UnityEngine;
using Zenject;

[ExecuteAlways]
public class BuildingGround : MonoBehaviour, ITouchable
{
    [SerializeField] private List<Vector3> _buildBoxes;
    [SerializeField] private float _quadSize;
    [SerializeField] private List<Transform> _blocks;
    private SelectedFrame _selectedFrame;

    private Dictionary<Vector3, BuildingCell> _buildCellsDict;
    private GameUIService _gameUIService;
    private TowerFactory _towerFactory;
    private PoolService _poolService;
    private BuildingCell _selectedCell;
    private bool _isTouched;

    private void ConstructBlocks()
    {
        _buildCellsDict = new();
        foreach (var item in _buildBoxes)
        {
            var cell = new BuildingCell(item);
            cell.Construct(_gameUIService, _towerFactory, _poolService);
            _buildCellsDict.Add(item, cell);
        }
    }

    [Inject]
    public void Construct(GameUIService gameService, SelectedFrame selectedFrame, TowerFactory towerFactory, PoolService poolService)
    {
        _gameUIService = gameService;
        _selectedFrame = selectedFrame;
        _towerFactory = towerFactory;
        ConstructBlocks();
    }

    [ContextMenu("SetBlocks")]
    public void SetBlocksEditor()
    {
        _buildBoxes.Clear();

        foreach (var item in _blocks)
        {
            _buildBoxes.Add(item.position);
        }
    }

    public void ClearAllBuildedTowers()
    {
        foreach (var item in _buildCellsDict)
        {
            var cell = item.Value;
            if(cell.GetPlacedTower() != null)
            {
                cell.ReleaseTower();
            }
        }
    }

    public bool IsTouched() => _isTouched;

    public void Touch(Vector3 touchPos)
    {
        if(_selectedCell != null) _selectedCell.SetActiveCell(false);

        _selectedCell = FindQuad(touchPos);

        if (_selectedCell != null)
        {
            _selectedCell.SetActiveCell(true);
            _gameUIService.SetBuildingCell(_selectedCell);

            var placedTower = _selectedCell.GetPlacedTower();
            var container = _selectedCell.GetContainer();

            if (placedTower)
            {
                placedTower.ShowRange();
                _gameUIService.ShowGameMenu();
                _gameUIService.GetBottomMenuInformator().SetEntityToPannelUpdate(container, TouchableType.Tower);
            }
            else
            {
                _gameUIService.ShowEmptyCellMenu();
            }
            _selectedFrame.EnableFrame();
            _selectedFrame.transform.position = _selectedCell.GetPosition();
        }
    }

    public void Untouch()
    {
        if (_selectedCell != null)
        {
            _selectedCell.SetActiveCell(false);

            var tower = _selectedCell.GetPlacedTower();
            if (tower != null)
                tower.HideRange();
            _selectedCell = null;
            _selectedFrame.DisableFrame();
        }
    }

    private BuildingCell FindQuad(Vector3 hitPoint)
    {
        float halfSize = _quadSize * 0.5f;
        BuildingCell cell = null;

        foreach (var center in _buildBoxes)
        {
            if (hitPoint.x >= center.x - halfSize && hitPoint.x <= center.x + halfSize &&
                hitPoint.z >= center.z - halfSize && hitPoint.z <= center.z + halfSize)
            {
                _buildCellsDict.TryGetValue(center, out cell);
                return cell;
            }
        }

        float minDist = float.MaxValue;
        Vector3 nearest = _buildBoxes[0];

        foreach (var center in _buildBoxes)
        {
            float dist = Vector3.SqrMagnitude(hitPoint - center);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = center;
                _buildCellsDict.TryGetValue(nearest, out cell);
                return cell;
            }
        }
        return cell;
    }
}
