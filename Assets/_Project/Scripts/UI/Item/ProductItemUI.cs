using _Project.Scripts.Model;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ProductItemUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Image _iconImage;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private Image _backgroundImage;

    private ProductData _productData;
    private ProductPopup _productPopup;
    private Canvas _canvas;
    private Camera _camera;
    private RectTransform _rectTransform;
    private RaycastHit2D _hit;
    private Transform _parent;
    private Vector2 _originalPos;
    private Vector2 _originalScale;
    private int _childIndex;

    private void Awake()
    {
        _camera = Camera.main;
    }

    public void Init(ProductData productData, ProductPopup productPopup, Canvas gameCanvas)
    {
        _rectTransform = GetComponent<RectTransform>();
        _parent = transform.parent;
        _canvas = gameCanvas;
        _productPopup = productPopup;
        _productData = productData;
        _iconImage.sprite = productData.CollectibleData.Icon;
        _nameText.text = productData.ProductName;
        _timeText.text = $"{productData.Timer}s";
        _originalPos = _rectTransform.anchoredPosition;
        _originalScale = _rectTransform.localScale;
        _childIndex = transform.GetSiblingIndex();
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        EnableDragMode();
    }

    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)_canvas.transform,
            eventData.position, _canvas.worldCamera, out var position);

        transform.position = _canvas.transform.TransformPoint(position);

        _productPopup.ClosePopup();
        TryPlant();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_parent == null) Destroy(gameObject);
        else
            DisableDragMode();
    }

    private void TryPlant()
    {
        var touchPos = _camera.ScreenToWorldPoint(Input.mousePosition);
        _hit = Physics2D.Raycast(touchPos, Vector2.zero);

        if (_hit.collider == null) return;
        _hit.collider.gameObject.TryGetComponent(out Field field);

        if (field != null && field.State.CurrentValue == Field.ProductionBuildingState.Idle)
            field.AddProduct(_productData);
    }

    private void EnableDragMode()
    {
        transform.SetParent(_canvas.transform);
        _nameText.enabled = false;
        _timeText.enabled = false;
        _backgroundImage.enabled = false;
        _rectTransform.localScale *= 2f;
    }

    private void DisableDragMode()
    {
        transform.SetParent(_parent.transform);
        _rectTransform.anchoredPosition = _originalPos;
        _nameText.enabled = true;
        _timeText.enabled = true;
        _backgroundImage.enabled = true;
        _rectTransform.localScale = _originalScale;
        transform.SetSiblingIndex(_childIndex);
    }
}