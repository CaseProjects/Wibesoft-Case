using _Project.Scripts;
using _Project.Scripts.Model;
using UnityEngine;
using UnityEngine.EventSystems;

public class CropUI : MonoBehaviour,IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private ProductData _data;

    private RectTransform _rectTransform;
    private Canvas _canvas;
    protected RaycastHit2D _hit;
    private Transform _parent;
    private Vector2 _originalPos;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvas = FindObjectOfType<Canvas>();
        _parent = transform.parent;
        _originalPos = _rectTransform.anchoredPosition;
    }
    /*
    public void Init(Canvas gameCanvas, CropData cropData, FieldProductInfoCell parent)
    {
        rectTrans = gameObject.GetComponent<RectTransform>();
        icon = gameObject.GetComponent<Image>();

        canvas = gameCanvas;
        data = cropData;
        icon.sprite = cropData.product.icon;
        cellParent = parent;
    }
    */


    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetParent(_canvas.transform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)_canvas.transform,
            eventData.position, _canvas.worldCamera, out var position);

        transform.position = _canvas.transform.TransformPoint(position);

        FindObjectOfType<ProductPopup>().ClosePopup();
        TryPlant();
    }

    public new void OnEndDrag(PointerEventData eventData)
    {
        if (_parent == null) Destroy(gameObject);
        else
        {
            transform.SetParent(_parent.transform);
            _rectTransform.anchoredPosition = _originalPos;
        }
    }

    void TryPlant()
    {
        Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _hit = Physics2D.Raycast(touchPos, Vector2.zero);

        if (_hit.collider == null) return;
        _hit.collider.gameObject.TryGetComponent(out Field field);

        if (field != null && field.State.CurrentValue == Building.ProductionBuildingState.Idle)
        {
            field.AddProduct(_data);
        }
    }
}