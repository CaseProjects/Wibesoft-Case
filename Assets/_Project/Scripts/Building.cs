using System;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Project.Scripts
{
    [RequireComponent(typeof(PolygonCollider2D))]
    public class Building : MonoBehaviour
    {
        [field: SerializeField]
        public SerializableReactiveProperty<ProductionBuildingState> State { get; private protected set; } =
            new(ProductionBuildingState.Idle);

        public BuildingData BuildingData;
        public BoundsInt BuildingArea;
        public string BuildingName;
        protected SpriteRenderer _spriteRenderer;
        public bool IsPlaced { get; private set; }

        private bool _isMouseDown;
        private float _mouseDownTime;

        private BuildingSystem _buildingSystem;
        private MoveState _moveState;
        private bool _setupComplete;

        private enum MoveState
        {
            FirstSetup,
            Idle,
            Moving,
        }

        public enum ProductionBuildingState
        {
            Idle,
            EarlyStage,
            Processing,
            Complete,
        }

        private protected void Awake()
        {
            _buildingSystem = FindObjectOfType<BuildingSystem>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            if (_moveState != MoveState.FirstSetup) return;

            if (Input.GetMouseButton(0))
            {
                MoveBuilding();
            }

            if (Input.GetMouseButtonUp(0))
            {
                _moveState = MoveState.Moving;
                OnMouseUp();
            }
        }

        public void Init(BuildingData buildingData)
        {
            BuildingData = buildingData;
            BuildingArea = buildingData.Area;
            BuildingName = buildingData.Name;
            // _moveState = MoveState.First;
            // _spriteRenderer.sprite = buildingData.Sprite;
        }

        public bool Placeable()
        {
            BoundsInt areaTemp = BuildingArea;
            areaTemp.position = _buildingSystem.GridLayout.LocalToCell(transform.position);
            areaTemp.position -= new Vector3Int(
                Mathf.CeilToInt((float)areaTemp.size.x),
                Mathf.CeilToInt((float)areaTemp.size.y),
                0);

            return _buildingSystem.ValidArea(areaTemp);
        }

        protected virtual void OnMouseDown()
        {
            //if (UICurtain.Instance.gameObject.activeSelf) return;
            if (State.CurrentValue == ProductionBuildingState.Processing)
                return;


            _isMouseDown = true;
            _mouseDownTime = Time.time;
        }

        protected void OnMouseDrag()
        {
            if (_moveState == MoveState.Idle && _isMouseDown && Time.time - _mouseDownTime >= 0.1f)
            {
                _moveState = MoveState.Moving;
                _isMouseDown = false;
                _buildingSystem.SetupBuilding(this);
            }

            if (_moveState == MoveState.Moving)
            {
                MoveBuilding();
            }
        }

        private void OnMouseUp()
        {
            if (_moveState == MoveState.Moving)
            {
                if (Placeable())
                {
                    _buildingSystem.Confirm();
                }
                else
                {
                    _buildingSystem.CancelMove();
                }

                _moveState = MoveState.Idle;
            }

            else if (State.CurrentValue == ProductionBuildingState.Idle && _setupComplete)
            {
                Vector3 topLeftCorner = GetTopLeftCorner(_spriteRenderer);
                float spriteHeight = _spriteRenderer.bounds.size.y;

                var adjustedWorldPos = topLeftCorner + new Vector3(0, spriteHeight * 0.5f, 0);

                FindObjectOfType<ProductPopup>().Init(adjustedWorldPos);
            }
        }
        Vector3 GetTopLeftCorner(SpriteRenderer sr)
        {
            Bounds bounds = sr.bounds;
            return new Vector3(bounds.min.x, bounds.max.y, bounds.max.z);
        }

        void MoveBuilding()
        {
            IsPlaced = false;
            _buildingSystem.MoveBuilding();
        }

        public void Place()
        {
            /*
            var areaTemp = BuildingArea;
            areaTemp.position = _buildingSystem.GridLayout.LocalToCell(transform.position);
            areaTemp.position -= new Vector3Int(
                Mathf.CeilToInt(areaTemp.size.x),
                Mathf.CeilToInt(areaTemp.size.y),
                0);
                */
            IsPlaced = true;
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
            _buildingSystem.SetTilesBlock(BuildingArea, TileType.White);
            if (_setupComplete is false)
            {
                CompleteSetup();
            }
        }

        private async UniTask CompleteSetup()
        {
            Debug.LogError("Ne");
            await UniTask.Delay(250);
            _setupComplete = true;
        }
    }
}