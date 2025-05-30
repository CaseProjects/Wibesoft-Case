﻿using UnityEngine;

namespace _Project.Scripts.Helpers.Extensions
{
    public static class GameObjectExtensions
    {
        public static T GetOrAddComponent<T>(this GameObject gameObject)
            where T : Component
        {
            var component = gameObject.GetComponent<T>();
            if (component == null) component = gameObject.AddComponent<T>();

            return component;
        }
    }
}