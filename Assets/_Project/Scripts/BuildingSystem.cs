using System.Collections.Generic;
using System.Linq;
using _Project.Scripts;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class BuildingSystem : SerializedMonoBehaviour
{
    [SerializeField] private Camera sceneCamera;

    [SerializeField] private LayerMask placementLayermask;

    [field: SerializeField] public GridLayout GridLayout;
    [SerializeField] private Tilemap _tempTileMap;

    [SerializeField] private Transform _buildingParent;
    private Vector3Int _prevPosition;

    private Building _tempBuilding;

    [SerializeField] private Dictionary<TileType, TileBase> _tileTypeMap;
    private TileBase[] _prevTileBase;
    private BoundsInt _prevTileArea;

    private BoundsInt _beforeMoveArea;
    private Vector3 _beforeMovePos;

    [ReadOnly] private bool _isBuildingMode;

    public bool ValidArea(BoundsInt area)
    {
        TileBase[] baseArray = _tempTileMap.GetTilesBlock(area);
        return baseArray.All(tile => tile == _tileTypeMap[TileType.Green]);
    }

    public void Confirm()
    {
        if (!_tempBuilding.Placeable()) return;

        _tempBuilding.Place();

        ResetPrev();
        CloseBuildingMode();
    }

    private void ResetPrev()
    {
        _tempBuilding = null;
        _prevTileArea = new BoundsInt();
        _prevPosition = new Vector3Int();
        _prevTileBase = null;
    }


    public void MoveBuilding()
    {
        if (!_tempBuilding) return;
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (_tempBuilding.IsPlaced) return;

        Vector2 touchPosition = sceneCamera.ScreenToWorldPoint(Input.mousePosition);
        var cellPosition = GridLayout.LocalToCell(touchPosition);

        if (_prevPosition == cellPosition) return;

        var worldPosition = GridLayout.CellToLocalInterpolated(cellPosition + new Vector3(1, 1, 0));
        _prevPosition = cellPosition;
        _tempBuilding.transform.position = worldPosition;
        TileFollowBuilding();
    }

    public void SetupBuilding(Building building)
    {
        _tempBuilding = building;
        _prevTileArea = building.BuildingArea;
        _beforeMoveArea = building.BuildingArea;
        _beforeMovePos = building.transform.position;
        
        //HorizontalUIHolder.Instance.confirmButton.onClick.AddListener(Confirm);
        //HorizontalUIHolder.Instance.cancelButton.onClick.AddListener(CancelMove);

        OpenBuildingMode();
    }

    public void CancelMove()
    {
        _tempBuilding.BuildingArea = _beforeMoveArea;
        _tempBuilding.transform.position = _beforeMovePos;
        ClearPrevious();
        CloseBuildingMode();

        _tempTileMap.SetTilesBlock(_tempBuilding.BuildingArea, SetTileBaseArrayValue(_tempBuilding.BuildingArea, TileType.White));
        ResetPrev();
    }

    void OpenBuildingMode()
    {
        _tempTileMap.gameObject.SetActive(true);
    }

    void CloseBuildingMode()
    {
        _isBuildingMode = false;
        _tempTileMap.gameObject.SetActive(false);
    }


    void TileFollowBuilding()
    {
        ClearPrevious();

        _tempBuilding.BuildingArea.position =
            GridLayout.WorldToCell(_tempBuilding.gameObject.transform.position) -
            new Vector3Int(
                Mathf.CeilToInt(_tempBuilding.BuildingArea.size.x),
                Mathf.CeilToInt(_tempBuilding.BuildingArea.size.y),
                0);
        var baseArray = _tempTileMap.GetTilesBlock(_tempBuilding.BuildingArea);
        var tileArray = new TileBase[baseArray.Length];

        for (int i = 0; i < baseArray.Length; i++)
        {
            if (baseArray[i] == _tileTypeMap[TileType.Empty])
            {
                tileArray[i] = _tileTypeMap[TileType.Green];
            }
            else
            {
                FillTiles(tileArray, TileType.Red);
                break;
            }
        }

        _prevTileBase = _tempTileMap.GetTilesBlock(_tempBuilding.BuildingArea);
        //Take tiles before setting new ones
        _tempTileMap.SetTilesBlock(_tempBuilding.BuildingArea, tileArray);
        _prevTileArea = _tempBuilding.BuildingArea;
    }

    void ClearPrevious()
    {
        /*
        if (_prevTileBase != null)
            foreach (var tileBase in _prevTileBase)
            {
                Debug.LogError(tileBase.name);
            }
            */

        _prevTileBase ??= SetTileBaseArrayValue(_prevTileArea, TileType.Empty);
        _tempTileMap.SetTilesBlock(_prevTileArea, _prevTileBase);
    }

    public void InstantiateConstruction(BuildingData buildingData)
    {
        OpenBuildingMode();

        var emptyBuilding = new GameObject
        {
            transform =
            {
                parent = _buildingParent,
                position = new Vector3(-1, -1, 0)
            },
            name = buildingData.Name,
        };

        var sRenderer = emptyBuilding.AddComponent<SpriteRenderer>();
        sRenderer.sprite = buildingData.Sprite;
        //sRenderer.material = ResourceManager.Instance.grayscale;
        sRenderer.sortingOrder = 1;
        sRenderer.color = new Color(255, 255, 255, 1);

        if (buildingData.Type == BuildingData.BuildingType.Field)
        {
            _tempBuilding = emptyBuilding.AddComponent<Field>();
        }
        else
        {
            _tempBuilding = emptyBuilding.AddComponent<Building>();
        }
        _tempBuilding.Init(buildingData);

       
    }

    private TileBase[] SetTileBaseArrayValue(BoundsInt area, TileType type)
    {
        var toReturn = new TileBase[area.size.x * area.size.y * area.size.z];
        FillTiles(toReturn, type);
        return toReturn;
    }
    
    public void SetTilesBlock(BoundsInt area, TileType type)
    {
        _tempTileMap.SetTilesBlock(area, SetTileBaseArrayValue(area, type));
    }

    void FillTiles(IList<TileBase> tiles, TileType type)
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            tiles[i] = _tileTypeMap[type];
        }
    }
}

public enum TileType
{
    Empty,
    White,
    Red,
    Green,
}