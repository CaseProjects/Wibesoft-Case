using UnityEngine;

[CreateAssetMenu(fileName = "BuildingData", menuName = "CustomObject/BuildingData")]
public class BuildingData : ScriptableObject
{
    [field: SerializeField] public string Id { get; private set; }
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Sprite Sprite { get; private set; }
    [field: SerializeField] public BuildingType Type { get; private set; }
    [field: SerializeField] public BoundsInt Area { get; private set; }
    [field: SerializeField] public int Timer { get; private set; }

    protected virtual void OnValidate()
    {
        var area = Area;
        area.size = new Vector3Int(Area.size.x, Area.size.y, 1);
        Area = area;
    }
    
    public enum BuildingType
    {
        Default,
        Field,
    }

    
}