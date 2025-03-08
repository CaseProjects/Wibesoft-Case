using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts;
using Events;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using Zenject;

public class BuildingSystem : SerializedMonoBehaviour
{
    [SerializeField] private Camera sceneCamera;

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

    private DiContainer _container;
    private SignalBus _signalBus;

    [Inject]
    private void Construct(DiContainer container, SignalBus signalBus)
    {
        _container = container;
        _signalBus = signalBus;
    }

    private void Awake()
    {
        _signalBus.Subscribe<InstantiateConstructionSignal>(OnInstantiateConstructionSignal);
    }

    private void OnDestroy()
    {
        _signalBus.Unsubscribe<InstantiateConstructionSignal>(OnInstantiateConstructionSignal);
    }

    public bool ValidArea(BoundsInt area)
    {
        TileBase[] baseArray = _tempTileMap.GetTilesBlock(area);
        return baseArray.All(tile => tile == _tileTypeMap[TileType.Green]);
    }

    public void Confirm()
    {
        if (!_tempBuilding.IsPlaceable()) return;

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

        OpenBuildingMode();
    }

    public void CancelMove()
    {
        _tempBuilding.BuildingArea = _beforeMoveArea;
        _tempBuilding.transform.position = _beforeMovePos;
        ClearPrevious();
        CloseBuildingMode();

        _tempTileMap.SetTilesBlock(_tempBuilding.BuildingArea,
            SetTileBaseArrayValue(_tempBuilding.BuildingArea, TileType.White));
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
        _tempTileMap.SetTilesBlock(_tempBuilding.BuildingArea, tileArray);
        _prevTileArea = _tempBuilding.BuildingArea;
    }

    void ClearPrevious()
    {
        _prevTileBase ??= SetTileBaseArrayValue(_prevTileArea, TileType.Empty);
        _tempTileMap.SetTilesBlock(_prevTileArea, _prevTileBase);
    }

    private void OnInstantiateConstructionSignal(InstantiateConstructionSignal signalData)
    {
        InstantiateConstruction(signalData.BuildingData);
    }

    private void InstantiateConstruction(BuildingData buildingData)
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
        sRenderer.sortingOrder = 1;
        sRenderer.color = new Color(255, 255, 255, 0.5f);

        _tempBuilding = buildingData.Type switch
        {
            BuildingData.BuildingType.Field => emptyBuilding.AddComponent<Field>(),
            BuildingData.BuildingType.RePlaceable => emptyBuilding.AddComponent<RePlaceableBuilding>(),
            _ => emptyBuilding.AddComponent<Building>()
        };

        InjectGameObjectDependencies(_tempBuilding);

        _tempBuilding.Init(this, buildingData);
    }


    private void InjectGameObjectDependencies<T>(T component) where T : Component
    {
        _container.InjectGameObjectForComponent<T>(component.gameObject);
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