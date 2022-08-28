using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Lean.Touch;

public class BuildCellInitializer : MonoBehaviour
{
    public GameObject Selected;

    [SerializeField] private GameObject _selectedFrame;
    [SerializeField] private BuildingCell _buildingCell;
    [SerializeField] private List<NewTower> _towersList = new List<NewTower>();
    [SerializeField] private List<Button> _emptyButtons = new List<Button>();
    [SerializeField] private float _touchDelay = 0.5f;
    [SerializeField] private float _touchInMs;
    [SerializeField] private float _distance;

    private NewTower _towerScript;
    private Enemy _enemyScript;
    private Camera _camera;
    private GameBottomPanel _gameBottomPanel;
    private int _count;
    private bool _isTouched;
    private Vector2 _tapDownPosition;
    private Vector2 _tapUpPosition;
    private protected void Awake()
    {       
        _touchInMs = _touchDelay;
        _count = _towersList.Count;
        _gameBottomPanel = GetComponent<GameBottomPanel>();
        _camera = Camera.main;
        _selectedFrame.SetActive(false);
        InitializeCell();
    }

    private protected void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            _tapUpPosition = Input.mousePosition;
            _isTouched = false;
            if (_touchInMs > 0 || Vector2.Distance(_tapDownPosition, _tapUpPosition) < 100f)
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    PointerEventData pointerData = new PointerEventData(EventSystem.current);
                    pointerData.position = Input.mousePosition;
                    List<RaycastResult> results = new List<RaycastResult>();
                    EventSystem.current.RaycastAll(pointerData, results);
                    Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out var hit))
                    {
                        Selected = hit.collider.gameObject;
                        _buildingCell = hit.collider.gameObject.GetComponent<BuildingCell>();

                        if (_buildingCell != null)
                        {
                            _selectedFrame.SetActive(true);
                            _selectedFrame.transform.position = Selected.transform.position;

                            if (_buildingCell._isEmpty)
                            {
                                _gameBottomPanel.ShowEmptyCellMenu();
                                InitializeCell();
                            }
                            else
                            {
                                _gameBottomPanel.ShowGameMenu();
                                _buildingCell.UpgradeInfo();
                            }
                            DisableEnemy();
                        }
                        else
                        {
                            _selectedFrame.SetActive(false);

                            if (Selected.layer == 3)
                            {
                                DisableEnemy();
                                _gameBottomPanel.ShowGameMenu();
                                _enemyScript = Selected.GetComponent<Enemy>();
                                _enemyScript.EnableSelectFrame();
                                _enemyScript.UpgradeInformation();
                            }

                            else
                            {
                                _selectedFrame.SetActive(false);                               
                                _gameBottomPanel.HideEmptyCellMenu();
                                _gameBottomPanel.HideGameMenu();

                                Selected = null;
                                DisableEnemy();
                            }
                        }
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _tapDownPosition = Input.mousePosition;
            _isTouched = true;
        }

        if (_isTouched)
        {
            if (_touchInMs >= 0)
            {
                _touchInMs -= Time.deltaTime;
            }
        }
        else
        {            
            _touchInMs = _touchDelay;
        }
    }

    public void DidsbleSelectFrame()
    {
        _selectedFrame.SetActive(false);
        Selected = null;
    }

    private void DisableEnemy()
    {
        if (_enemyScript != null)
        {
            _enemyScript.DisableSelectFrame();
            _enemyScript = null;
        }
    }

    private void InitializeCell()
    {
        for (int i = 0; i < _count; i++)
        {
            _towerScript = _towersList[i].GetComponent<NewTower>();
            _emptyButtons[i].gameObject.SetActive(true);
            var towerName = _towerScript.GetTowerName();
            var towerImage = _towerScript.GetTowerImage();
            _emptyButtons[i].image.sprite = towerImage;
            _emptyButtons[i].GetComponentInChildren<Text>().text = towerName.Remove(towerName.Length - 1);

            var button = _emptyButtons[i].GetComponent<TowerPlacer>();

            button.tower = _towersList[i];
            button.buildingCellObject = Selected;
        }
    }
}
