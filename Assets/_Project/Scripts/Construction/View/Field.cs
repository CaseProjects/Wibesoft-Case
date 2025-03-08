using System.ComponentModel;
using _Project.Scripts;
using _Project.Scripts.Model;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Events;
using R3;
using UnityEngine;
using Zenject;

public class Field : RePlaceableBuilding
{
    [field: SerializeField]
    public SerializableReactiveProperty<ProductionBuildingState> State { get; private protected set; } =
        new(ProductionBuildingState.Idle);

    private ProductData _productData;
    [Inject] private SignalBus _signalBus;

    public enum ProductionBuildingState
    {
        Idle,
        EarlyStage,
        Processing,
        Complete,
    }

    protected override void OnMouseDown()
    {
        if (State.CurrentValue == ProductionBuildingState.Processing)
            return;

        base.OnMouseDown();
    }

    protected override void OnMouseDrag()
    {
        if (State.CurrentValue is not (ProductionBuildingState.Idle or ProductionBuildingState.Complete))
            return;

        base.OnMouseDrag();
    }

    protected override void OnMouseUp()
    {
        ResetMouseDown();

        if (_buildState == BuildState.Moving)
        {
            OnMouseUpWhenMovement();
        }

        else if (_buildState == BuildState.Idle && State.CurrentValue == ProductionBuildingState.Idle && _setupComplete)
        {
            var center = GetCenter(_spriteRenderer);
            var spriteHeight = _spriteRenderer.bounds.size.y;

            var adjustedWorldPos = center + new Vector3(0, spriteHeight * 0.5f, 0);

            _signalBus.Fire(new SetActiveProductPopupSignal(true, adjustedWorldPos));
        }

        else if (IsPlaced && TimerObject != null)
        {
            _signalBus.Fire(new SetActiveTimerUISignal(TimerObject));
        }
    }

    private Vector3 GetCenter(SpriteRenderer sprite)
    {
        var bounds = sprite.bounds;
        return new Vector3(bounds.min.x, bounds.max.y, bounds.max.z);
    }


    public void AddProduct(ProductData data)
    {
        _productData = data;
        ProcessProduct();
    }

    private async UniTask ProcessProduct()
    {
        TimerObject = gameObject.AddComponent<TimerObject>();
        TimerObject.Init(_productData.Timer, _productData.ProductName);

        State.Value = ProductionBuildingState.EarlyStage;
        await AnimateTransition(_productData.StateSpriteDataMap[0].StateSprite, 0.3f);

        await TimerObject.HalfTimeTask;

        State.Value = ProductionBuildingState.Processing;
        await AnimateTransition(_productData.StateSpriteDataMap[1].StateSprite, 0.5f);

        await TimerObject.CompletionTask;

        State.Value = ProductionBuildingState.Complete;
        await AnimateTransition(_productData.StateSpriteDataMap[2].StateSprite, 0.7f);
    }

    public async void HarvestProduct()
    {
        State.Value = ProductionBuildingState.Idle;
        _spriteRenderer.sprite = BuildingData.Sprite;
        _signalBus.Fire(new CollectedProductSignal(_productData.CollectibleData, transform.position));
        _productData = null;
        /*var harvestSequence = DOTween.Sequence()
            .Append(_spriteRenderer.transform.DOScale(1.1f, 0.15f))
            .Join(_spriteRenderer.DOColor(new Color(1.3f, 1.3f, 1.3f, 1), 0.1f))
            .Append(_spriteRenderer.transform.DOScale(0f, 0.3f).SetEase(Ease.InBack))
            .Join(_spriteRenderer.DOFade(0, 0.25f))
            .OnComplete(() => {
                //_spriteRenderer.sprite = emptyFieldSprite;
                _spriteRenderer.color = Color.white;
            });

        await harvestSequence.AsyncWaitForCompletion();

        // Boş tarla için toprak efekti
        _spriteRenderer.transform.localScale = Vector3.zero;
        await _spriteRenderer.transform.DOScale(1f, 0.5f)
            .SetEase(Ease.OutBounce)
            .AsyncWaitForCompletion();*/
    }

    private async UniTask AnimateTransition(Sprite newSprite, float duration)
    {
        var scaleTween = _spriteRenderer.transform.DOPunchScale(Vector3.one * 0.05f, duration);
        var colorTween = _spriteRenderer.DOColor(Color.gray, duration / 2)
            .SetLoops(2, LoopType.Yoyo);

        await _spriteRenderer.DOFade(0, duration / 3).OnComplete(() =>
        {
            _spriteRenderer.sprite = newSprite;
            _spriteRenderer.DOFade(1, duration / 3);
        }).AsyncWaitForCompletion().AsUniTask();


        await UniTask.WhenAll(
            scaleTween.AsyncWaitForCompletion().AsUniTask(),
            colorTween.AsyncWaitForCompletion().AsUniTask()
        );
    }
}