using System;
using DG.Tweening;
using R3;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarFiller : MonoBehaviour
{
    [SerializeField] private float _time;

    private Image _image;
    private float _targetValue;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _targetValue = _image.fillAmount > 0.5f ? 0 : 1;
    }

    private void Start()
    {
        Observable.Timer(TimeSpan.FromSeconds(0.3f)).Subscribe(x =>
        {
            DOTween.To(() => _image.fillAmount, x => _image.fillAmount = x, _targetValue, _time);
        }).AddTo(gameObject);
    }
}