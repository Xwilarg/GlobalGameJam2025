using UnityEngine;

namespace GGJ.SO
{
    [CreateAssetMenu(menuName = "ScriptableObject/GameInfo", fileName = "GameInfo")]
    public class GameInfo : ScriptableObject
    {
        [Header("Movements")]
        [Tooltip("Speed of the player")] public float Speed;

        [Header("Interactions")]
        [Tooltip("Distance between player and center point of the interaction")] public float InteractionDistance;
        [Tooltip("Size of the interaction area")] public float InteractionSize;

        [Header("Money")]
        [Tooltip("Min/Max the selling price can reach")] public Range MinMaxPrice;
        [Tooltip("Curve for the base prices")] public AnimationCurve PriceCurve;
    }

    [System.Serializable]
    public class Range
    {
        public int Min, Max;
    }
}