using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
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

        [SerializeField]
        private PlayerInputManager _inputManager;

        public GamePhase GamePhase { private set; get; }

        public UnityEvent<GamePhase> OnNextPhase { get; } = new();

        private readonly List<Dirt> _dirts = new();

        private void Awake()
        {
            Instance = this;

            _infoText.text = "Press any button to join...";
            _timerText.text = string.Empty;
        }

        private void Start()
        {
#if !UNITY_EDITOR
            SceneManager.LoadScene("Lobby", LoadSceneMode.Additive);
            _inputManager.EnableJoining();
#else
            StartCoroutine(SwitchToLobbyDebug());
#endif
        }

        private IEnumerator SwitchToLobbyDebug()
        {
            yield return SceneManager.UnloadSceneAsync(ResourceManager.Instance.GameInfo.GameLevel.Name);
            OnResetAll();
            yield return SceneManager.LoadSceneAsync("Lobby", LoadSceneMode.Additive);
            _inputManager.EnableJoining();
        }

        public void OnResetAll()
        {
            PlayerManager.Instance.ClearRegisters();
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
                else if (GamePhase == GamePhase.GameEnded)
                {
                    StartCoroutine(BackToLobby());
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
            _inputManager.DisableJoining();
            StartCoroutine(ReloadScene());
            PlayerManager.Instance.ResetAllPlayers();
        }

        private IEnumerator BackToLobby()
        {
            for (int i = 10; i > 0; i--)
            {
                _infoText.text = $"Redirecting to lobby in {i}...";
                yield return new WaitForSeconds(1f);
            }
            _infoText.text = string.Empty;
            GamePhase = GamePhase.LobbyPreparation;
            yield return SceneManager.UnloadSceneAsync(ResourceManager.Instance.GameInfo.GameLevel.Name);
            OnResetAll();
            yield return SceneManager.LoadSceneAsync("Lobby", LoadSceneMode.Additive);
            _inputManager.EnableJoining();
        }

        private IEnumerator ReloadScene()
        {
            yield return SceneManager.UnloadSceneAsync("Lobby");
            OnResetAll();
            yield return SceneManager.LoadSceneAsync(ResourceManager.Instance.GameInfo.GameLevel.Name, LoadSceneMode.Additive);
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
        PriceCrash,
        GameEnded
    }
}