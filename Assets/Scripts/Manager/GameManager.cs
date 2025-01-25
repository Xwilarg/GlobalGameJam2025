using System.Collections;
using System.Collections.Generic;
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
                    StartCoroutine(ReloadScene(_targetScene));
                    PlayerManager.Instance.ResetAllPlayers();
                }
            }
        }

        private IEnumerator ReloadScene(string scene)
        {
            yield return SceneManager.UnloadSceneAsync(scene);
            yield return SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
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