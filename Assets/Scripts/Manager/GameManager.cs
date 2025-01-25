using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace GGJ.Manager
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { private set; get; }

        public GamePhase GamePhase { private set; get; }

        public UnityEvent<GamePhase> OnNextPhase { get; } = new();

        private void Awake()
        {
            Instance = this;

#if !UNITY_EDITOR
            SceneManager.LoadScene("01");
#endif
        }

        public void SetPhase(GamePhase targetPhase)
        {
            if (targetPhase > GamePhase)
            {
                GamePhase = targetPhase;
                OnNextPhase.Invoke(GamePhase);
            }
        }
    }

    public enum GamePhase
    {
        LobbyPreparation,
        PriceRaise,
        PriceCrash
    }
}