using UnityEngine;

public class OrderInLayerMatchY : MonoBehaviour
{
    SpriteRenderer[] spriteRenderers;

    [SerializeField] bool isStatic = true;

    void Awake()
    {
        Cache();
    }

    void OnEnable()
    {
        UpdateOrderInLayer();
    }

    void Cache()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>(true);
    }

    void Update()
    {
        if (!isStatic)
            UpdateOrderInLayer();
    }

    void UpdateOrderInLayer()
    {
        foreach (SpriteRenderer sr in spriteRenderers)
            sr.sortingOrder = -(int)transform.position.y;
    }
}
