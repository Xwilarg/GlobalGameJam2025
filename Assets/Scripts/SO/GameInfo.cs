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
        [Tooltip("Curve for the base prices during increase phase")] public AnimationCurve RaisePriceCurve;
        [Tooltip("Curve for the base prices during crash phase")] public AnimationCurve CrashPriceCurve;

        [Header("Time")]
        [Tooltip("How much time in seconds a day is")] public float DayDuration;
        [Tooltip("Duration of the phase where prices increase in days")] public int RaisePhaseDuration;
        [Tooltip("Duration of the phase where prices crashes in days")] public int CrashPhaseDuration;

        [Header("Economy")]
        [Tooltip("% variation per flower sold (before variance with average)")] [Range(1, 100)] public int PercSellVariation;
    }

    [System.Serializable]
    public class Range
    {
        public int Min, Max;
    }
}