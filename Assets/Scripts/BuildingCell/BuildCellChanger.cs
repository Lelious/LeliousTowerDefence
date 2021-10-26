using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BuildCellChanger : MonoBehaviour
{
    public GameObject Selected;

    [SerializeField] private GameObject _selectedFrame;
    [SerializeField] private BuildingCell _buildingCell;
    [SerializeField] private List<GameObject> _towersList = new List<GameObject>();
    [SerializeField] private List<Button> _emptyButtons = new List<Button>();
    [SerializeField] private float _touchDelay = 0.2f;
   
    private Tower _towerScript;
    private Camera _camera;
    private GameBottomPanel _gameBottomPanel;
    private int _count;
    private bool _isTouched;
    [SerializeField] private float _touchInMs;
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
        if (Input.touchCount > 0)
        {
            if (_touchInMs >= 0)
            {
                _touchInMs -= Time.deltaTime;
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            if (_touchInMs > 0)
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
                        _selectedFrame.transform.position = new Vector3(Selected.transform.position.x, 0.61f, Selected.transform.position.z);

                        if (_buildingCell != null)
                        {
                            _selectedFrame.SetActive(true);
                            _selectedFrame.transform.position = new Vector3(Selected.transform.position.x, 0.61f, Selected.transform.position.z);

                            if (_buildingCell._isEmpty)
                            {
                                _gameBottomPanel.ShowEmptyCellMenu();
                                _gameBottomPanel.HideGameMenu();
                                InitializeCell();
                            }
                            else
                            {
                                _gameBottomPanel.HideEmptyCellMenu();
                                _gameBottomPanel.ShowGameMenu();
                                _buildingCell.UpgradeInfo();
                            }
                        }
                        else
                        {
                            _selectedFrame.SetActive(false);
                            _gameBottomPanel.HideGameMenu();
                            _gameBottomPanel.HideEmptyCellMenu();
                        }
                    }
                }
            }
            _touchInMs = _touchDelay;
        }
    }

    public void DidsbleSelectFrame()
    {
        _selectedFrame.SetActive(false);
    }

    private void InitializeCell()
    {
        for (int i = 0; i < _count; i++)
        {
            _towerScript = _towersList[i].GetComponent<Tower>();
            _emptyButtons[i].gameObject.SetActive(true);
            _emptyButtons[i].image.sprite = _towerScript.mainImage;
            _emptyButtons[i].GetComponentInChildren<Text>().text = _towerScript.towerName;

            var button = _emptyButtons[i].GetComponent<TowerPlacer>();

            button.tower = _towersList[i];
            button.buildingCellObject = Selected;
        }
    }
}
