using System;
using _Project.Scripts;
using _Project.Scripts.Helpers.Extensions.R3;
using Events;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class TimerPopupUI : MonoBehaviour
{
    [Inject] private SignalBus _signalBus;
    [SerializeField] private Slider _progress;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _timeLeft;
    [SerializeField] private RectTransform _rectTransform;
    private IDisposable _clickOutsideDisposable;
    private Camera _mainCam;

    private bool _countdown;
    private TimerObject _timerObject;

    protected void Awake()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        _mainCam = Camera.main;
        _signalBus.Subscribe<SetActiveTimerUISignal>(OnSetActiveTimerUISignal);
    }

    private void OnDestroy()
    {
        _signalBus.Unsubscribe<SetActiveTimerUISignal>(OnSetActiveTimerUISignal);
    }

    private void OnSetActiveTimerUISignal(SetActiveTimerUISignal data)
    {
        ShowTimer(data.TimerObjectObject);
    }

    void Update()
    {
        if (!_countdown) return;

        if (_timerObject.TimeLeft == 0)
        {
            HideTimer();
            return;
        }

        _progress.value = (float)(1 - _timerObject.TimeLeft / _timerObject.Duration.TotalSeconds);
        _timeLeft.text = _timerObject.TimeLeftString();
    }

    private void ShowTimer(TimerObject timerObjectObject)
    {
        _timerObject = timerObjectObject;

        if (_timerObject == null) HideTimer();

        _nameText.text = _timerObject.TimerName;
        gameObject.SetActive(true);

        timerObjectObject.TryGetComponent(out Collider2D collider2D);

        _clickOutsideDisposable = _rectTransform.ClickOutsideAsObservable().Subscribe(_ => HideTimer());
        if (collider2D != null)
        {
            var timerPosition = CalculateTimerPosition(collider2D);
            _rectTransform.anchoredPosition = timerPosition;
        }
        else
        {
            transform.position = _mainCam.WorldToScreenPoint
                (timerObjectObject.transform.position + Vector3.down);
        }

        _countdown = true;
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
    }

    private void HideTimer()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        _timerObject = null;
        _countdown = false;
        _clickOutsideDisposable.Dispose();
    }

    private Vector2 CalculateTimerPosition(Collider2D collider2D)
    {
        var bound = collider2D.bounds;
        var offsetX = bound.extents.y * 0.7f;
        var offsetY = bound.extents.y * 0f;

        var bottomPos = new Vector3(bound.center.x + offsetX, bound.min.y + offsetY, bound.center.z);
        var screenPos = _mainCam.WorldToScreenPoint(bottomPos);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform.parent as RectTransform,
            screenPos,
            null,
            out var uiPos
        );
        return uiPos;
    }
}