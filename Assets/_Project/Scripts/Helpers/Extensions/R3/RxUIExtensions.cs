using R3;
using UnityEngine;

namespace _Project.Scripts.Helpers.Extensions.R3
{
    public static class RxUIExtensions
    {
        public static Observable<Unit> ClickOutsideAsObservable(this RectTransform rectTransform)
        {
            if (rectTransform == null || rectTransform.gameObject == null) return Observable.Empty<Unit>();

            return rectTransform.gameObject.GetOrAddComponent<ObservableClickOutsideTrigger>()
                .ClickOutsideAsObservable(rectTransform);
        }
    }
}