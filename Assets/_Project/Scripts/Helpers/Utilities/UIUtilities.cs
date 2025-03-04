using UnityEngine;
using UnityEngine.EventSystems;

namespace Helpers.Utilities
{
    public static class UIUtilities
    {
        public static bool IsFingerOverUI()
        {
#if UNITY_ANDROID || UNITY_IOS
            return (EventSystem.current.IsPointerOverGameObject(-1));
#else 
            return Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
#endif
        }
        
        public static bool IsFingerOverObject() => EventSystem.current.currentSelectedGameObject != null;
    }
}