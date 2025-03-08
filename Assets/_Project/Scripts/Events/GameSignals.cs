using _Project.Scripts;
using _Project.Scripts.Model;
using UnityEngine;

namespace Events
{
    public struct InstantiateConstructionSignal
    {
        public BuildingData BuildingData { get; }

        public InstantiateConstructionSignal(BuildingData buildingData)
        {
            BuildingData = buildingData;
        }
    }

    public struct SetActiveTimerUISignal
    {
        public TimerObject TimerObjectObject { get; }

        public SetActiveTimerUISignal(TimerObject timerObjectObject)
        {
            TimerObjectObject = timerObjectObject;
        }
    }

    public struct SetActiveProductPopupSignal
    {
        public bool IsActive { get; }
        public Vector3? Position { get; }

        public SetActiveProductPopupSignal(bool isActive, Vector3? position = null)
        {
            IsActive = isActive;
            Position = position;
        }
    }

    public struct SetActiveBuildingItemsPopupSignal
    {
        public bool? IsActive { get; }

        public SetActiveBuildingItemsPopupSignal(bool? isActive = null)
        {
            IsActive = isActive;
        }
    }
    public struct CollectedProductSignal
    {
        public CollectibleData CollectibleData { get; }
        public Vector3 Position { get; }

        public CollectedProductSignal(CollectibleData collectibleData, Vector3 position)
        {
            CollectibleData = collectibleData;
            Position = position;
        }
    }
}