using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private BuildingSystem _buildingSystem;
    [SerializeField] private BuildingData _buildingData;

    public void OnPointerDown(PointerEventData eventData)
    {
        _buildingSystem.InstantiateConstruction(_buildingData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
    }
}