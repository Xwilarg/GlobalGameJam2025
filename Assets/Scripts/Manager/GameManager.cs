using UnityEngine;

namespace GGJ.Manager
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { private set; get; }

        public GamePhase GamePhase { private set; get; } = GamePhase.PriceRaise;

        private void Awake()
        {
            Instance = this;
        }
    }

    public enum GamePhase
    {
        LobbyPreparation,
        PriceRaise,
        PriceCrash
    }
}