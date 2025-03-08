using Helpers.Extensions;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Helpers.Utilities
{
    public static class UIUtilities
    {
        private static readonly Vector3[] _tempVectors = new Vector3[4];

        public static bool IsFingerOverUI()
        {
#if UNITY_ANDROID || UNITY_IOS
            return (EventSystem.current.IsPointerOverGameObject(-1));
#else
            return Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
#endif
        }

        public static bool IsFingerOverObject() => EventSystem.current.currentSelectedGameObject != null;


        public static bool CheckCollisionOnPosition((Vector2 min, Vector2 max) handUvPos)
        {
            var mousePos = Input.mousePosition;
            var mouseUvPos = new Vector2(mousePos.x / Screen.width, mousePos.y / Screen.height);
            mouseUvPos = mouseUvPos.RoundVector();
            handUvPos.min = handUvPos.min.RoundVector();
            handUvPos.max = handUvPos.max.RoundVector();

            return mouseUvPos.x >= handUvPos.min.x && mouseUvPos.x <= handUvPos.max.x &&
                   mouseUvPos.y >= handUvPos.min.y &&
                   mouseUvPos.y <= handUvPos.max.y;
        }

        public static (Vector2 min, Vector2 max) GetRectMinMaxViewport(RectTransform rectTransform)
        {
            rectTransform.GetWorldCorners(_tempVectors);

            var minScreen = RectTransformUtility.WorldToScreenPoint(null, _tempVectors[0]);
            var maxScreen = RectTransformUtility.WorldToScreenPoint(null, _tempVectors[2]);

            var rectMinUV = new Vector2(minScreen.x / Screen.width, minScreen.y / Screen.height);
            var rectMaxUV = new Vector2(maxScreen.x / Screen.width, maxScreen.y / Screen.height);
            return (rectMinUV, rectMaxUV);
        }
    }
}