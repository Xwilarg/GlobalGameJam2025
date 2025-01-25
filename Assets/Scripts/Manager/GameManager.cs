using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GGJ.Manager
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { private set; get; }

        public GamePhase GamePhase { private set; get; }

        public UnityEvent<GamePhase> OnNextPhase { get; } = new();

        private readonly List<Dirt> _dirts = new();

        private const string _targetScene = "01";

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

                if (GamePhase == GamePhase.PriceRaise)
                {
                    foreach (var d in _dirts)
                    {
                        d.Clear();
                    }
                }
            }
        }

        public void Register(Dirt d)
        {
            _dirts.Add(d);
        }
    }

    public enum GamePhase
    {
        LobbyPreparation,
        PriceRaise,
        PriceCrash
    }
}