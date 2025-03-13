using Helpers.Utilities;
using R3;
using R3.Triggers;
using UnityEngine;

public class ObservableClickOutsideTrigger : ObservableTriggerBase
{
    private Subject<Unit> _onClickOutside;
    private RectTransform _rectTransform;

    private void Update()
    {
        if (_rectTransform.gameObject.activeSelf && Input.GetMouseButtonDown(0) &&
            !UIUtilities.IsMouseClickInsideRect(_rectTransform))
        {
            _onClickOutside?.OnNext(Unit.Default);
        }
    }

    public Observable<Unit> ClickOutsideAsObservable(RectTransform rectTransform)
    {
        _rectTransform = rectTransform;
        return _onClickOutside ??= new Subject<Unit>();
    }

    protected override void RaiseOnCompletedOnDestroy()
    {
        _onClickOutside?.OnCompleted();
    }
}