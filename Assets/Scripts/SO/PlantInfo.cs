using UnityEngine;

namespace GGJ.SO
{
    [CreateAssetMenu(menuName = "ScriptableObject/PlantInfo", fileName = "PlantInfo")]
    public class PlantInfo : ScriptableObject
    {
        [Tooltip("How long before plant need water")] public float NeedWaterDeltaTime = 3;
    }
}

