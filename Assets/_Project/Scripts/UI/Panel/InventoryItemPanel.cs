using System;
using _Project.Scripts.Model;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.UI
{
    public class InventoryItemPanel : MonoBehaviour
    {
        [Inject] private Settings _settings;
        [Inject] private DiContainer _container;
        [SerializeField] private RectTransform _inventoryItemUiParent;

        private void Awake()
        {
            foreach (var collectibleData in _settings.Collectibles)
            {
                var buildingItemUI =
                    _container.InstantiatePrefabForComponent<InventoryItemUI>(_settings.InventoryItemUIPrefab,
                        _inventoryItemUiParent);
                buildingItemUI.Init(collectibleData);
            }
        }

        [Serializable]
        public class Settings
        {
            public InventoryItemUI InventoryItemUIPrefab;
            public CollectibleData[] Collectibles;
        }
    }
}