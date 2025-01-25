using UnityEngine;

namespace GGJ.SO
{
    [CreateAssetMenu(menuName = "ScriptableObject/FlowerInfo", fileName = "FlowerInfo")]
    public class FlowerInfo : ScriptableObject
    {
        [Tooltip("All body flower images (without bulb) in growth order")] public Sprite[] BodySprites;
        [Tooltip("All bulb flower images in growth order")] public Sprite[] BulbSprites;
        [Tooltip("How long before flower need water")] public float NeedWaterDeltaTime = 3;
    }
}

