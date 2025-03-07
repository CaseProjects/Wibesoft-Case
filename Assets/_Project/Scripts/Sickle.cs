using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Sickle : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] RectTransform rectTrans;
    [SerializeField] Canvas canvas;

    Vector2 originalPos;
    RaycastHit2D hit;


    private void Awake()
    {
        originalPos = rectTrans.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetParent(canvas.transform);
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position ;
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)canvas.transform,
            eventData.position, canvas.worldCamera, out position);
        
        transform.position = canvas.transform.TransformPoint(position);
        Detect();
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        rectTrans.anchoredPosition = originalPos;
    }
    
    
    void Detect()
    {
        Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        hit = Physics2D.Raycast(touchPos, Vector2.zero);

        if (hit.collider == null) return;
        hit.collider.gameObject.TryGetComponent(out Field field);
        
        if (field != null && field.State.CurrentValue== Building.ProductionBuildingState.Complete)
        {
            field.HarvestProduct();
        }
    }
}