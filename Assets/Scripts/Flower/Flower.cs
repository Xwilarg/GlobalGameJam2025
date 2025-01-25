using GGJ.Manager;
using GGJ.Player;
using GGJ.Prop;
using UnityEngine;


public class Flower : MonoBehaviour, IInteractible
{
    [SerializeField] SpriteRenderer bodySpriteRenderer;
    [SerializeField] SpriteRenderer bulbSpriteRenderer;

    [SerializeField] GameObject needWaterPopup;
    [SerializeField] GameObject needCutPopup;

    [SerializeField] GameObject cutedFlowerPrefab;


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
    }
    void SetNeedWaterTrue() => SetNeedWater(true);

    void SetNeedCut(bool needCut)
    {
        this.needCut = needCut;
        needCutPopup.SetActive(needCut);
    }

    void SetNeedCutTrue() => SetNeedCut(true);

    int MaxGrounthLvl { get => ResourceManager.Instance.FlowerInfo.BodySprites.Length - 1; }
    bool IsAtMaxGrounthLvl { get => growthLvl == MaxGrounthLvl; }


    void Start()
    {
        UpdatebodySpriteRenderers();

        needWaterPopup.SetActive(needWater);
        needCutPopup.SetActive(needCut);

        Invoke("SetNeedWaterTrue", ResourceManager.Instance.FlowerInfo.NeedWaterDeltaTime);
    }


    void OnDestroy()
    {
        if (dirt)
            dirt.Flower = null;
    }

    void UpdatebodySpriteRenderers()
    {
        bodySpriteRenderer.sprite = ResourceManager.Instance.FlowerInfo.BodySprites[growthLvl];
        bulbSpriteRenderer.sprite = ResourceManager.Instance.FlowerInfo.BulbSprites[growthLvl];
    }

    void Water()
    {
        if (!needWater)
            return;

        SetNeedWater(false);

        SetGrowthLvl(growthLvl + 1);

        Invoke(IsAtMaxGrounthLvl ? "SetNeedCutTrue" : "SetNeedWaterTrue", ResourceManager.Instance.FlowerInfo.NeedWaterDeltaTime);
    }

    void Cut()
    {
        if (!needCut)
            return;

        Instantiate(cutedFlowerPrefab, transform.position, transform.rotation);

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
        return (needCut && pc.CarriedObject.GameObject.name == "Scissors") ||
               (needCut && pc.CarriedObject.GameObject.name == "Water");
    }
}
