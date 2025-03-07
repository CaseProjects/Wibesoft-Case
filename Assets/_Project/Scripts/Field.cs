using _Project.Scripts;
using _Project.Scripts.Model;
using Cysharp.Threading.Tasks;
using R3;

public class Field : Building
{
    private ProductData _productData;

    protected new void Awake()
    {
        base.Awake();
        State.Subscribe(OnStateChanged).AddTo(gameObject);
    }

    private void OnStateChanged(ProductionBuildingState productionBuildingState)
    {
        ChangeSpriteByState(productionBuildingState);
    }

    private void ChangeSpriteByState(ProductionBuildingState productionBuildingState)
    {
        _spriteRenderer.sprite = productionBuildingState switch
        {
            ProductionBuildingState.EarlyStage => _productData.StateSpriteDataMap[0].StateSprite,
            ProductionBuildingState.Processing => _productData.StateSpriteDataMap[1].StateSprite,
            ProductionBuildingState.Complete => _productData.StateSpriteDataMap[2].StateSprite,
            _ => _spriteRenderer.sprite
        };
    }

    public void AddProduct(ProductData data)
    {
        _productData = data;
        ProcessProduct();
    }

    private async UniTask ProcessProduct()
    {
        var remainingTime = _productData.Timer * 1000;

        State.Value = ProductionBuildingState.EarlyStage;
        await UniTask.Delay(remainingTime / 2);
        State.Value = ProductionBuildingState.Processing;
        await UniTask.Delay(remainingTime / 2);
        State.Value = ProductionBuildingState.Complete;
    }
    
    public void HarvestProduct()
    {
        State.Value = ProductionBuildingState.Idle;
        _spriteRenderer.sprite= BuildingData.Sprite;
        _productData = null;
    }
}