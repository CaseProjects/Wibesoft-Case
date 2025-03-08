using Events;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace _Project.Scripts.UI
{
    public class BuildingItemUI : MonoBehaviour, IPointerDownHandler
    {
        [Inject] private SignalBus _signalBus;
        [SerializeField] private Image _iconImage;
        [SerializeField] private TextMeshProUGUI _nameText;

        private BuildingData _buildingData;
        private BuildingItemsPopup _buildingItemsPopup;

        public void Init(BuildingData buildingData, BuildingItemsPopup buildingItemsPopup, Canvas gameCanvas)
        {
            _buildingItemsPopup = buildingItemsPopup;
            _buildingData = buildingData;
            _iconImage.sprite = buildingData.Sprite;
            _nameText.text = buildingData.Name;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _signalBus.Fire<InstantiateConstructionSignal>(new(_buildingData));
            _buildingItemsPopup.ClosePopup();
        }
    }
}