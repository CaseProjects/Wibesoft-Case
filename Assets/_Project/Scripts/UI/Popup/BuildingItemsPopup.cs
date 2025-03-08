using System;
using System.ComponentModel;
using _Project.Scripts.Helpers.Extensions.R3;
using _Project.Scripts.UI;
using DG.Tweening;
using Events;
using R3;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class BuildingItemsPopup : MonoBehaviour
{
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private RectTransform _scrollRectTransform;
    [SerializeField] private RectTransform _productUIParent;
    [SerializeField] private Canvas _gameCanvas;
    private bool _isActive;

    private Settings _settings;
    private SignalBus _signalBus;
    private DiContainer _container;
    private IDisposable _clickOutsideDisposable;

    [Inject]
    private void Construct(Settings settings, SignalBus signalBus, DiContainer container)
    {
        _settings = settings;
        _signalBus = signalBus;
        _container = container;
    }

    private void Awake()
    {
        _isActive = false;
        _signalBus.Subscribe<SetActiveBuildingItemsPopupSignal>(OnSetActiveProductPopupSignal);
        InitializeBuildingItems();
    }


    private void OnDestroy()
    {
        _signalBus.Unsubscribe<SetActiveBuildingItemsPopupSignal>((OnSetActiveProductPopupSignal));
    }

    private void OnSetActiveProductPopupSignal(SetActiveBuildingItemsPopupSignal signalData)
    {
        if (signalData.IsActive.HasValue)
        {
            SetActivePopup(signalData.IsActive.Value);
            return;
        }

        SetActivePopup(!_isActive);
    }


    private void SetActivePopup(bool isActive)
    {
        if (isActive && !_isActive)
            EnablePopup();
        else if (!isActive)
            ClosePopup();
    }

    private void InitializeBuildingItems()
    {
        foreach (var buildingData in _settings.Buildings)
        {
            var buildingItemUI =
                _container.InstantiatePrefabForComponent<BuildingItemUI>(_settings.BuildingItemUIPrefab,
                    _productUIParent);
            buildingItemUI.Init(buildingData, this, _gameCanvas);
        }
    }

    private void EnablePopup()
    {
        _isActive = true;
        _scrollRect.gameObject.SetActive(true);
        _scrollRect.verticalNormalizedPosition = 1;
        _scrollRectTransform.DOMoveX(0, 0.25f).OnComplete(() =>
            _clickOutsideDisposable = _scrollRectTransform.ClickOutsideAsObservable().Subscribe(_ => ClosePopup()));
    }

    public void ClosePopup()
    {
        _isActive = false;
        _scrollRectTransform.DOAnchorPos(Vector2.zero, 0.25f);
        _clickOutsideDisposable?.Dispose();
    }

    [Serializable]
    public class Settings
    {
        public BuildingItemUI BuildingItemUIPrefab;
        public BuildingData[] Buildings;
    }
}