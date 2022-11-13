using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class UITapRegistrator : MonoBehaviour, IPointerClickHandler
{
    private GameUIService _gameInformationMenu;

    [Inject]
    private void Construct(GameUIService gameInformationMenu) => _gameInformationMenu = gameInformationMenu;    

    public void OnPointerClick(PointerEventData eventData) => _gameInformationMenu.RegisterTapOnUI();    
}
