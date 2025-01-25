using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace GGJ.Manager
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { private set; get; }

        [SerializeField]
        private TMP_Text _infoText;

        [SerializeField]
        private TMP_Text _timerText;

        public GamePhase GamePhase { private set; get; }

        public UnityEvent<GamePhase> OnNextPhase { get; } = new();

        private readonly List<Dirt> _dirts = new();

        private const string _targetScene = "01";

        private void Awake()
        {
            Instance = this;

            _infoText.text = "Press any button to join...";
            _timerText.text = "";

#if !UNITY_EDITOR
            SceneManager.LoadScene("01");
#endif
        }

        public void ShowReadyPendingText()
        {
            _infoText.text = "Waiting for players to plant a tulip";
        }

        public void SetPhase(GamePhase targetPhase)
        {
            if (targetPhase > GamePhase)
            {
                GamePhase = targetPhase;
                OnNextPhase.Invoke(GamePhase);

                if (GamePhase == GamePhase.PriceRaise)
                {
                    StartCoroutine(ReadyupTimer());
                }
            }
        }

        private IEnumerator ReadyupTimer()
        {
            for (int i = 3; i > 0; i--)
            {
                _timerText.text = i.ToString();
                yield return new WaitForSeconds(1f);
            }
            _timerText.text = "";
            _infoText.text = string.Empty;
            StartCoroutine(ReloadScene(_targetScene));
            PlayerManager.Instance.ResetAllPlayers();
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