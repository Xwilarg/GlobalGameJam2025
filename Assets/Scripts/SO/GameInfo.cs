using Eflatun.SceneReference;
using UnityEngine;

namespace GGJ.SO
{
    [CreateAssetMenu(menuName = "ScriptableObject/GameInfo", fileName = "GameInfo")]
    public class GameInfo : ScriptableObject
    {
        [Header("Movements")]
        [Tooltip("Speed of the player")] public float Speed;
        [Tooltip("Speed of the player when carrying an object")] public float SpeedWhenCarrying;

        [Header("Interactions")]
        [Tooltip("Distance between player and center point of the interaction")] public float InteractionDistance;
        [Tooltip("Size of the interaction area")] public float InteractionSize;
        [Tooltip("Stun duration in seconds")] public float StunDuration;
        [Tooltip("Stun force")] public float StunForce;
        [Tooltip("Time the sprite change to transparent when we take a hit (in seconds)")] public float HitEffectTime;

        [Header("Money")]
        [Tooltip("Min/Max the selling price can reach")] public Range MinMaxPrice;
        [Tooltip("Curve for the base prices during increase phase")] public AnimationCurve RaisePriceCurve;
        [Tooltip("Curve for the base prices during crash phase")] public AnimationCurve CrashPriceCurve;
        [Tooltip("Price coef want you sell the plant of a other player"), Range(0, 1)] public float OtherPlayerPlantPriceCoef = 0.5f;

        [Header("Time")]
        [Tooltip("How much time in seconds a day is")] public float DayDuration;
        [Tooltip("Duration of the phase where prices increase in days")] public int RaisePhaseDuration;
        [Tooltip("Duration of the phase where prices crashes in days")] public int CrashPhaseDuration;

        [Header("Economy")]
        [Tooltip("% variation per flower sold (before variance with average)")] [Range(1, 100)] public int PercSellVariation;

        [Header("Audio")]
        public AnimationCurve AudioCurveIncrease;
        public AnimationCurve AudioCurveDecrease;

        [Header("Plant")]
        [Tooltip("How long before plant need water")] public float NeedWaterDeltaTime = 3;

        [Header("Levels")]
        public SceneReference GameLevel;
    }

    [System.Serializable]
    public class Range
    {
        public int Min, Max;
    }
}