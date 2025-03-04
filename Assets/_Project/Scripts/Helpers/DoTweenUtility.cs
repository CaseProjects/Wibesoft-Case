using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

[HideMonoScript]
public class DoTweenUtility : MonoBehaviour
{
    [SerializeField] [HorizontalGroup("BOOL", LabelWidth = 75)]
    private bool _canMove;

    [SerializeField] [HorizontalGroup("BOOL", LabelWidth = 75)]
    private bool _canRotate;

    [SerializeField] [HorizontalGroup("BOOL", LabelWidth = 75)]
    private bool _canScale;

    private readonly IList<Tween> _tweenSequence = new List<Tween>();


    #region MoveSettings

    [ShowIfGroup("_canMove")]
    [FoldoutGroup("_canMove/MoveTweenSettings")]
    [HideLabel]
    [LabelText("RectTransform")]
    [SerializeField]
    private bool _isRectTransformMove;

    [ShowIfGroup("_canMove")] [FoldoutGroup("_canMove/MoveTweenSettings")] [SerializeField]
    private bool _canLoopWithReverseMove;


    [ShowIfGroup("_canMove")]
    [FoldoutGroup("_canMove/MoveTweenSettings")]
    [CustomContextMenu("SetPositionTo/GlobalPosition", nameof(SetPositionToGlobalTransform))]
    [CustomContextMenu("SetPositionTo/LocalPosition", nameof(SetPositionToLocalTransform))]
    [CustomContextMenu("SetPositionTo/RectPosition", nameof(SetPositionToRectTransform))]
    [SerializeField]
    private Vector3 _endPos;

    [ShowIfGroup("_canMove")] [FoldoutGroup("_canMove/MoveTweenSettings")] [SerializeField]
    private float _moveDuration;

    [ShowIfGroup("_canMove")]
    [FoldoutGroup("_canMove/MoveTweenSettings")]
    [HideLabel]
    [LabelText("EaseType")]
    [SerializeField]
    private Ease _easeTypeMove;

    #endregion

    #region RotationSettings

    [ShowIfGroup("_canRotate")] [FoldoutGroup("_canRotate/RotateTweenSettings")] [SerializeField]
    private bool _canLoopRotation;


    [ShowIfGroup("_canRotate")] [FoldoutGroup("_canRotate/RotateTweenSettings")] [SerializeField]
    private bool _canReverseRotation;

    [ShowIfGroup("_canRotate")]
    [FoldoutGroup("_canRotate/RotateTweenSettings")]
    [SerializeField]
    [CustomContextMenu("SetRotationTo/LocalRotation", nameof(SetRotationToLocalEuler))]
    private Vector3 _rotateTo;

    [ShowIfGroup("_canRotate")] [FoldoutGroup("_canRotate/RotateTweenSettings")] [SerializeField]
    private float _rotateDuration;


    [ShowIfGroup("_canRotate")]
    [FoldoutGroup("_canRotate/RotateTweenSettings")]
    [HideLabel]
    [LabelText("EaseType")]
    [SerializeField]
    private Ease _easeTypeRotation;

    #endregion

    #region ScaleSettings

    [ShowIfGroup("_canScale")] [FoldoutGroup("_canScale/ScaleTweenSettings")] [SerializeField]
    private bool _canLoopScale;

    [ShowIfGroup("_canScale")]
    [FoldoutGroup("_canScale/ScaleTweenSettings")]
    [SerializeField]
    [CustomContextMenu("SetScaleTo/LocalScale", nameof(SetScaleToLocalScale))]
    private Vector3 _scaleTo;

    [ShowIfGroup("_canScale")] [FoldoutGroup("_canScale/ScaleTweenSettings")] [SerializeField]
    private float _scaleDuration;

    [ShowIfGroup("_canScale")]
    [FoldoutGroup("_canScale/ScaleTweenSettings")]
    [HideLabel]
    [LabelText("EaseType")]
    [SerializeField]
    private Ease _easeTypeScale;

    #endregion


    #region CustomContextActions

    private void SetPositionToGlobalTransform()
    {
        _endPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    private void SetPositionToLocalTransform()
    {
        _endPos = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
    }

    private void SetPositionToRectTransform()
    {
        if (TryGetComponent(out RectTransform rectTransform))
            _endPos = new Vector3(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y, 0);
    }

    private void SetRotationToLocalEuler()
    {
        _rotateTo = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y,
            transform.localEulerAngles.z);
    }

    private void SetScaleToLocalScale()
    {
        _scaleTo = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    #endregion


    private void Start()
    {
        SetupMoveTween();
        SetupRotateTween();
        SetupScaleTween();
    }


    private void SetupMoveTween()
    {
        if (!_canMove) return;
        if (_isRectTransformMove)
        {
            if (_canLoopWithReverseMove)
                _tweenSequence.Add(transform.GetComponent<RectTransform>().DOAnchorPos(_endPos, _moveDuration)
                    .SetEase(_easeTypeMove)
                    .SetLoops(-1, LoopType.Yoyo));
            else
                _tweenSequence.Add(transform.GetComponent<RectTransform>().DOAnchorPos(_endPos, _moveDuration)
                    .SetEase(_easeTypeMove));
        }
        else
        {
            if (_canLoopWithReverseMove)
                _tweenSequence.Add(transform
                    .DOLocalMove(_endPos, _moveDuration)
                    .SetEase(_easeTypeMove)
                    .SetLoops(-1, LoopType.Yoyo));
            else
                _tweenSequence.Add(transform
                    .DOLocalMove(_endPos, _moveDuration)
                    .SetEase(_easeTypeMove));
        }
    }

    private void SetupRotateTween()
    {
        if (!_canRotate) return;
        if (_canLoopRotation)
        {
            if (_canReverseRotation)
                _tweenSequence.Add(transform
                    .DOLocalRotate(_rotateTo, _rotateDuration,
                        RotateMode.FastBeyond360)
                    .SetEase(_easeTypeRotation).SetLoops(-1, LoopType.Yoyo));
            else
                _tweenSequence.Add(transform
                    .DOLocalRotate(_rotateTo, _rotateDuration,
                        RotateMode.FastBeyond360)
                    .SetEase(_easeTypeRotation).SetLoops(-1, LoopType.Restart));
        }
        else
        {
            if (_canReverseRotation)
                _tweenSequence.Add(transform
                    .DOLocalRotate(_rotateTo, _rotateDuration,
                        RotateMode.FastBeyond360)
                    .SetEase(_easeTypeRotation).SetLoops(2, LoopType.Yoyo));
            else
                _tweenSequence.Add(transform
                    .DOLocalRotate(_rotateTo, _rotateDuration,
                        RotateMode.FastBeyond360)
                    .SetEase(_easeTypeRotation));
        }
    }


    private void SetupScaleTween()
    {
        if (!_canScale) return;
        if (_canLoopScale)
            _tweenSequence.Add(transform.DOScale(_scaleTo, _scaleDuration)
                .SetEase(_easeTypeScale).SetLoops(-1, LoopType.Yoyo));
        else
            _tweenSequence.Add(transform.DOScale(_scaleTo, _scaleDuration)
                .SetEase(_easeTypeScale));
    }

    #region Enable&Destroy

    private void OnEnable()
    {
        foreach (var tween in _tweenSequence)
        {
            tween.Play();
        }
    }

    private void OnDisable()
    {
        foreach (var tween in _tweenSequence)
        {
            tween.Pause();
        }
    }

    private void OnDestroy()
    {
        foreach (var tween in _tweenSequence)
        {
            tween.Pause();
        }
    }

    #endregion
}