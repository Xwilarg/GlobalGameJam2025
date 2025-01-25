using GGJ.Manager;
using GGJ.Player;
using GGJ.Prop;
using UnityEngine;


public class Plant : MonoBehaviour, IInteractible
{
    [SerializeField] SpriteRenderer bodySpriteRenderer;
    [SerializeField] SpriteRenderer bulbSpriteRenderer;

    [SerializeField] GameObject needWaterPopup;
    [SerializeField] GameObject needCutPopup;

    [SerializeField] GameObject cutedPlantPrefab;

    [SerializeField, Tooltip("All plant images (without flower) in growth order")] Sprite[] plantSprites;
    [SerializeField, Tooltip("All plant flower images in growth order")] Sprite[] flowerSprites;

    int growthLvl = 0;
    bool needWater = false;
    bool needCut = false;
    Dirt dirt;

    public Dirt Dirt { set => dirt = value; }


    void SetGrowthLvl(int growthLvl)
    {
        this.growthLvl = Mathf.Clamp(growthLvl, 0, MaxGrounthLvl);
        UpdatebodySpriteRenderers();
    }

    void SetNeedWater(bool needWater)
    {
        this.needWater = needWater;

        needWaterPopup.SetActive(needWater);

        if (needWater)
            Invoke("Water", 1); // TODO : test
    }
    void SetNeedWaterTrue() => SetNeedWater(true);

    void SetNeedCut(bool needCut)
    {
        this.needCut = needCut;
        needCutPopup.SetActive(needCut);

        if (needCut)
            Invoke("Cut", 1); // TODO : test
    }

    void SetNeedCutTrue() => SetNeedCut(true);

    int MaxGrounthLvl { get => plantSprites.Length - 1; }
    bool IsAtMaxGrounthLvl { get => growthLvl == MaxGrounthLvl; }

    public void SetBulbColor(Color color)
    {
        bulbSpriteRenderer.color = color;
    }

    void Start()
    {
        UpdatebodySpriteRenderers();

        needWaterPopup.SetActive(needWater);
        needCutPopup.SetActive(needCut);

        Invoke("SetNeedWaterTrue", ResourceManager.Instance.PlantInfo.NeedWaterDeltaTime);
    }


    void OnDestroy()
    {
        if (dirt)
            dirt.Plant = null;
    }

    void UpdatebodySpriteRenderers()
    {
        bodySpriteRenderer.sprite = plantSprites[growthLvl];
        bulbSpriteRenderer.sprite = flowerSprites[growthLvl];
    }

    void Water()
    {
        if (!needWater)
            return;

        SetNeedWater(false);

        SetGrowthLvl(growthLvl + 1);

        Invoke(IsAtMaxGrounthLvl ? "SetNeedCutTrue" : "SetNeedWaterTrue", ResourceManager.Instance.PlantInfo.NeedWaterDeltaTime);
    }

    void Cut()
    {
        if (!needCut)
            return;

        Instantiate(cutedPlantPrefab, transform.position, transform.rotation);

        Destroy(gameObject);
    }


  
    void IInteractible.Interact(PlayerController pc)
    {
        if (needCut)
            Cut();

        else if (needWater)
            Water();
    }

    public bool CanInteract(PlayerController pc)
    {
        return (needCut && pc.CarriedObject.GameObject && pc.CarriedObject.CanCut) ||
               (needWater && pc.CarriedObject.GameObject && pc.CarriedObject.CanWater);
    }
}
