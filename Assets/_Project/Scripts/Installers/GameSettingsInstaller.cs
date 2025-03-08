using System;
using _Project.Scripts.UI;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Installers
{
    [CreateAssetMenu(fileName = "GameSettingsInstaller", menuName = "Installers/GameSettingsInstaller")]
    [InlineEditor()]
    public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
    {
        [SerializeField] private UISettings _uiSettings;
        public override void InstallBindings()
        {
            InstallUiSettings();
        }

        private void InstallUiSettings()
        {
            Container.BindInstance(_uiSettings.ProductPopup);
            Container.BindInstance(_uiSettings.BuildingItemsPopup);
            Container.BindInstance(_uiSettings.InventoryItemPanel);
        }

        [Serializable]
        private class UISettings
        {
            public ProductPopup.Settings ProductPopup;
            public BuildingItemsPopup.Settings BuildingItemsPopup;
            public InventoryItemPanel.Settings InventoryItemPanel;
        }
    }
}