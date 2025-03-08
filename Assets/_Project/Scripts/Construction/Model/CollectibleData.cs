using UnityEngine;

namespace _Project.Scripts.Model
{
    [CreateAssetMenu(fileName = "Collectible", menuName = "CustomObject/Collectible")]
    public class CollectibleData : ScriptableObject
    {
        [field: SerializeField] public string ItemName { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
    }
}