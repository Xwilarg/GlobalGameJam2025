using UnityEngine;

public class OrderInLayerMatchY : MonoBehaviour
{
    SpriteRenderer[] spriteRenderers;
    int[] spriteRenderersOffset;

    [SerializeField] bool isStatic = true;

    [SerializeField]
    private bool _ignoreChild;

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

        if (_ignoreChild)
        {
            spriteRenderers = new SpriteRenderer[] { GetComponent<SpriteRenderer>() };
        }
        else
        {
            spriteRenderers = GetComponentsInChildren<SpriteRenderer>(true);
            spriteRenderersOffset = new int[spriteRenderers.Length];
            for (int i = 0; i < spriteRenderers.Length; i++)
                spriteRenderersOffset[i] = spriteRenderers[i].sortingOrder;
        }
    }

    void Update()
    {
        if (!isStatic)
            UpdateOrderInLayer();
    }

    void UpdateOrderInLayer()
    {
        if (_ignoreChild)
        {
            spriteRenderers[0].sortingOrder = -(int)(transform.position.y * 1000);
        }
        else
        {
            for (int i = 0; i < spriteRenderers.Length; i++)
                spriteRenderers[i].sortingOrder = -(int)(transform.position.y * 1000) + spriteRenderersOffset[i];
        }
    }
}
