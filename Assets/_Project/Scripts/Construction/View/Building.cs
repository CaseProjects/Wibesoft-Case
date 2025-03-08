using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace _Project.Scripts
{
    [RequireComponent(typeof(PolygonCollider2D))]
    public class Building : MonoBehaviour
    {
        public BuildingData BuildingData;
        public BoundsInt BuildingArea;
        public string BuildingName;
        protected SpriteRenderer _spriteRenderer;
        public bool IsPlaced { get; private set; }

        private protected BuildingSystem _buildingSystem;
        private protected BuildState _buildState;
        private protected bool _setupComplete;

        private protected TimerObject TimerObject;

        private protected enum BuildState
        {
            FirstSetup,
            Idle,
            Moving,
        }

        private protected void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private protected void Update()
        {
            if (_buildState != BuildState.FirstSetup) return;

            if (Input.GetMouseButton(0))
            {
                MoveBuilding();
            }

            if (Input.GetMouseButtonUp(0))
            {
                _buildState = BuildState.Moving;
                OnMouseUp();
            }
        }

        public void Init(BuildingSystem buildingSystem, BuildingData buildingData)
        {
            _buildingSystem = buildingSystem;
            BuildingData = buildingData;
            BuildingArea = buildingData.Area;
            BuildingName = buildingData.Name;
            _spriteRenderer.sprite = buildingData.Sprite;
            _buildState = BuildState.FirstSetup;
        }

        public bool IsPlaceable()
        {
            var areaTemp = BuildingArea;
            areaTemp.position = _buildingSystem.GridLayout.LocalToCell(transform.position);
            areaTemp.position -= new Vector3Int(
                Mathf.CeilToInt(areaTemp.size.x),
                Mathf.CeilToInt(areaTemp.size.y),
                0);

            return _buildingSystem.ValidArea(areaTemp);
        }

        protected virtual void OnMouseDown()
        {
        }

        protected virtual void OnMouseOver()
        {
        }

        protected virtual void OnMouseDrag()
        {
        }

        protected virtual void OnMouseUp()
        {
        }

        protected void MoveBuilding()
        {
            IsPlaced = false;
            _buildingSystem.MoveBuilding();
        }

        public void Place()
        {
            IsPlaced = true;
            _spriteRenderer.sortingOrder = 1;
            _spriteRenderer.DOColor(new Color(255, 255, 255, 1), 0.5f);
            _buildingSystem.SetTilesBlock(BuildingArea, TileType.White);
            if (_setupComplete is false)
            {
                CompleteSetup();
            }
        }

        private async UniTask CompleteSetup()
        {
            await UniTask.Delay(250);
            _setupComplete = true;
        }
    }
}