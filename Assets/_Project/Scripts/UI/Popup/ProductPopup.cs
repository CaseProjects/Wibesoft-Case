using System;
using _Project.Scripts.Helpers.Extensions.R3;
using _Project.Scripts.Model;
using Events;
using R3;
using UnityEngine;
using Zenject;

public class ProductPopup : MonoBehaviour
{
    [SerializeField] private RectTransform _rect;
    [SerializeField] private RectTransform _productUIParent;
    [SerializeField] private Canvas _gameCanvas;
    private Camera _camera;
    private Settings _settings;
    private SignalBus _signalBus;

    [Inject]
    private void Construct(Settings settings, SignalBus signalBus)
    {
        _settings = settings;
        _signalBus = signalBus;
    }

    private void Awake()
    {
        _camera = Camera.main;
        _rect.ClickOutsideAsObservable().Subscribe(_ => ClosePopup());
        _signalBus.Subscribe<SetActiveProductPopupSignal>(OnSetActiveProductPopupSignal);
        InitializeCrops();
    }

    private void OnDestroy()
    {
        _signalBus.Unsubscribe<SetActiveProductPopupSignal>((OnSetActiveProductPopupSignal));
    }

    private void OnSetActiveProductPopupSignal(SetActiveProductPopupSignal signalData)
    {
        if (signalData.IsActive)
            EnablePopup(signalData.Position.Value);
        else
            ClosePopup();
    }

    private void InitializeCrops()
    {
        foreach (var productData in _settings.Products)
        {
            var cropUI = Instantiate(_settings._productItemPrefab, _productUIParent);
            cropUI.Init(productData, this, _gameCanvas);
        }
    }

    private void EnablePopup(Vector3 position)
    {
        _rect.position = _camera.WorldToScreenPoint(position);
        _rect.gameObject.SetActive(true);
    }

    public void ClosePopup()
    {
        _rect.gameObject.SetActive(false);
    }

    [Serializable]
    public class Settings
    {
        public ProductItemUI _productItemPrefab;
        public ProductData[] Products;
    }
}