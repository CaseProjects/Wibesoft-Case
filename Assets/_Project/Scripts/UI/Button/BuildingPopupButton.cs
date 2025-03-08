using Events;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class BuildingPopupButton : MonoBehaviour, IPointerDownHandler
{
    [Inject] private SignalBus _signalBus;

    public void OnPointerDown(PointerEventData eventData)
    {
        _signalBus.Fire(new SetActiveBuildingItemsPopupSignal());
    }
}