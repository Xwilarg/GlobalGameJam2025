using UnityEngine;

namespace GGJ.SO
{
    [CreateAssetMenu(menuName = "ScriptableObject/PlayerInfo", fileName = "PlayerInfo")]
    public class PlayerInfo : ScriptableObject
    {
        [Header("Movements")]
        [Tooltip("Speed of the player")] public float Speed;

        [Header("Interactions")]
        [Tooltip("Distance between player and center point of the interaction")] public float InteractionDistance;
        [Tooltip("Size of the interaction area")] public float InteractionSize;
    }
}