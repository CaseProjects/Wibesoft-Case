using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ProductPopup : MonoBehaviour
{
    [SerializeField] private RectTransform _rect;

    private bool _isActive;

    private void Awake()
    {
        _rect.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Eğer popup açıksa ve popup dışına tıklanmışsa kapat
            if (_rect.gameObject.activeSelf && !IsPointerOverUI())
            {
                ClosePopup();
                return;
            }

            // Vector3 worldPoint = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            // Vector3Int cellPosition = GetTilePosition(worldPoint);
            // ShowPopup(cellPosition);
        }
    }

    public void ClosePopup()
    {
        _rect.gameObject.SetActive(false);
    }

    bool IsPointerOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    public void Init(Vector3 worldPos)
    {
        //Offset the popup to the left corner 

        var worldToScreenPoint = Camera.main.WorldToScreenPoint(worldPos);
        //var offset = new Vector3(-_rect.rect.width / 4, _rect.rect.height / 4);
        //worldToScreenPoint += offset;
        _rect.position = worldToScreenPoint;
        _rect.gameObject.SetActive(true);
    }
}