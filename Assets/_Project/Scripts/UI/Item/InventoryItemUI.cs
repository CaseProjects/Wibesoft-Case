using _Project.Scripts.Model;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Events;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class InventoryItemUI : MonoBehaviour
{
    [Inject] private SignalBus _signalBus;
    [SerializeField] private Image _iconImage;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _countText;

    private CollectibleData _collectibleData;
    private int _count;
    private Vector3 _defaultScale;
    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    public void Init(CollectibleData collectibleData)
    {
        _count = 0;
        _collectibleData = collectibleData;
        _iconImage.sprite = collectibleData.Icon;
        _nameText.text = collectibleData.ItemName;
        _defaultScale = transform.localScale;
        transform.localScale = Vector3.zero;
        _signalBus.GetStream<CollectedProductSignal>().Where(collectedProductSignal =>
                collectedProductSignal.CollectibleData == collectibleData)
            .Subscribe(OnCollected);
    }

    private async void OnCollected(CollectedProductSignal signalData)
    {
        if (_count == 0)
            await transform.DOScale(_defaultScale, 0.1f).AsyncWaitForCompletion().AsUniTask();

        await InstantiateInventoryIconAndMove(signalData.Position);
        _count++;
        _countText.text = _count.ToString();
    }

    private async UniTask InstantiateInventoryIconAndMove(Vector3 position)
    {
        var icon = Instantiate(_iconImage, transform);
        icon.transform.SetParent(transform.parent.parent.parent, true);
        icon.rectTransform.position = _camera.WorldToScreenPoint(position);
        icon.transform.DOScale(icon.transform.localScale * 2, 0.25f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.Linear);
        await UniTask.Delay(250);
        await icon.transform.DOMove(_iconImage.transform.position, 2.5f).AsyncWaitForCompletion().AsUniTask();
        Destroy(icon.gameObject);
    }
}