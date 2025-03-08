using UnityEngine;
using UnityEngine.EventSystems;

public class SickleTool : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private Canvas _canvas;

    private Vector2 _originalPos;
    private RaycastHit2D _hit;
    private Camera _camera;


    private void Awake()
    {
        _camera = Camera.main;
        _originalPos = _rectTransform.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)_canvas.transform,
            eventData.position, _canvas.worldCamera, out var position);

        transform.position = _canvas.transform.TransformPoint(position);
        Detect();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition = _originalPos;
    }


    private void Detect()
    {
        var touchPos = _camera.ScreenToWorldPoint(Input.mousePosition);
        _hit = Physics2D.Raycast(touchPos, Vector2.zero);

        if (_hit.collider == null) return;
        _hit.collider.gameObject.TryGetComponent(out Field field);

        if (field != null && field.State.CurrentValue == Field.ProductionBuildingState.Complete)
        {
            field.HarvestProduct();
        }
    }
}