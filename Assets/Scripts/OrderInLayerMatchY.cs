using UnityEngine;

public class OrderInLayerMatchY : MonoBehaviour
{
    SpriteRenderer[] spriteRenderers;
    int[] spriteRenderersOffset;

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
        spriteRenderersOffset = new int[spriteRenderers.Length];

        for (int i = 0; i < spriteRenderers.Length; i++)
            spriteRenderersOffset[i] = spriteRenderers[i].sortingOrder;
    }

    void Update()
    {
        if (!isStatic)
            UpdateOrderInLayer();
    }

    void UpdateOrderInLayer()
    {
        for (int i = 0; i < spriteRenderers.Length; i++)
            spriteRenderers[i].sortingOrder = -(int)(transform.position.y * 100) + spriteRenderersOffset[i];
    }
}
