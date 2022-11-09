using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class UITapRegistrator : MonoBehaviour, IPointerClickHandler
{
    private GameInformationMenu _gameInformationMenu;

    [Inject]
    private void Construct(GameInformationMenu gameInformationMenu) => _gameInformationMenu = gameInformationMenu;    

    public void OnPointerClick(PointerEventData eventData) => _gameInformationMenu.RegisterTapOnUI();    
}
