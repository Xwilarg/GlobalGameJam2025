using GGJ.Manager;
using GGJ.Player;
using GGJ.Prop;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Plant : MonoBehaviour, IInteractible
{
    [SerializeField] SpriteRenderer plantSpriteRenderer;
    [SerializeField] SpriteRenderer flowerSpriteRenderer;

    [SerializeField] GameObject needWaterPopup;
    [SerializeField] GameObject needCutPopup;

    [SerializeField] CutedPlant cutedPlantPrefab;

    [SerializeField, Tooltip("All plant images (without flower) in growth order")] Sprite[] plantSprites;
    [SerializeField, Tooltip("All plant flower images in growth order")] Sprite[] flowerSprites;

    int growthLvl = 0;
    bool needWater = false;
    bool needCut = false;
    Dirt dirt;

    public Dirt Dirt { set; get; }


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

    int MaxGrounthLvl { get => plantSprites.Length - 1; }
    bool IsAtMaxGrounthLvl { get => growthLvl == MaxGrounthLvl; }

    public void SetFlowerColor(Color color)
    {
        flowerSpriteRenderer.color = color;
    }

    void Start()
    {
        UpdatebodySpriteRenderers();

        needWaterPopup.SetActive(needWater);
        needCutPopup.SetActive(needCut);

        Invoke("SetNeedWaterTrue", ResourceManager.Instance.GameInfo.NeedWaterDeltaTime);
    }


    void DestroyGameObject()
    {
        if (dirt)
            dirt.Plant = null;

        Destroy(gameObject);
    }

    void UpdatebodySpriteRenderers()
    {
        plantSpriteRenderer.sprite = plantSprites[growthLvl];
        flowerSpriteRenderer.sprite = flowerSprites[growthLvl];
    }

    void Water()
    {
        if (!needWater)
            return;

        SetNeedWater(false);

        SetGrowthLvl(growthLvl + 1);

        if (IsAtMaxGrounthLvl) SetNeedCut(true);
        else Invoke("SetNeedWaterTrue", ResourceManager.Instance.GameInfo.NeedWaterDeltaTime);
        //Invoke(IsAtMaxGrounthLvl ? "SetNeedCutTrue" : "SetNeedWaterTrue", ResourceManager.Instance.GameInfo.NeedWaterDeltaTime);
    }

    void Cut()
    {
        if (!needCut)
            return;

        CutedPlant cutedPlant = Instantiate(cutedPlantPrefab, transform.position, transform.rotation);
        cutedPlant.transform.parent = GameManager.Instance.SceneTransform;

        cutedPlant.SetFlowerColor(flowerSpriteRenderer.color);

        DestroyGameObject();
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
        if (GameManager.Instance.GamePhase == GamePhase.LobbyPreparation) return false; // Can't interact with flower during rup phase

        return (needCut && pc.CarriedObject != null && pc.CarriedObject.CanCut) ||
               (needWater && pc.CarriedObject != null && pc.CarriedObject.CanWater);
    }
}
