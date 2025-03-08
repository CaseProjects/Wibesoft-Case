using System;
using UnityEngine;

namespace _Project.Scripts.Model
{
    [CreateAssetMenu(fileName = "ProductData", menuName = "CustomObject/ProductData")]
    public class ProductData : ScriptableObject
    {
        [field: SerializeField] public CollectibleData CollectibleData { get; private set; }
        [field: SerializeField] public string ProductName;
        [field: SerializeField] public int Timer { get; private set; }
        [field: SerializeField] public StateSpriteData[] StateSpriteDataMap { get; private set; }
    }

    [Serializable]
    public class StateSpriteData
    {
        [field: SerializeField] public ProductStates State { get; private set; }
        [field: SerializeField] public Sprite StateSprite { get; private set; }
    }

    public enum ProductStates
    {
        START,
        PROCESSING,
        COMPLETE,
    }
}