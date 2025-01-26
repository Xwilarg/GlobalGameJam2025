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
        Animator _timerAnimator;

        [SerializeField]
        private PlayerInputManager _inputManager;

        [SerializeField]
        private GameObject _pauseDisplay;

        public GamePhase GamePhase { private set; get; }

        public UnityEvent<GamePhase> OnNextPhase { get; } = new();

        private readonly List<Dirt> _dirts = new();

        public Transform SceneTransform { private set; get; }
        public bool IsPaused => _pauseDisplay.activeInHierarchy;

        private void Awake()
        {
            Instance = this;

            _infoText.text = "Press any button to join...";
            _timerText.text = string.Empty;
            _timerAnimator = _timerText.GetComponent<Animator>();
        }

        private void Start()
        {
            StartCoroutine(SwitchToLobbyDebug());
        }

        public void TogglePause()
        {
            _pauseDisplay.SetActive(!_pauseDisplay.activeInHierarchy);
            Time.timeScale = _pauseDisplay.activeInHierarchy ? 0f : 1f;
        }

        private IEnumerator SwitchToLobbyDebug()
        {
#if UNITY_EDITOR
            yield return SceneManager.UnloadSceneAsync(ResourceManager.Instance.GameInfo.GameLevel.Name);
#endif
            OnResetAll();
            PlayerManager.Instance.ResetAllPlayers();
            yield return SceneManager.LoadSceneAsync("Lobby", LoadSceneMode.Additive);
            SceneTransform = new GameObject("Container").transform;
            SceneManager.MoveGameObjectToScene(SceneTransform.gameObject, SceneManager.GetSceneByName("Lobby"));
            _inputManager.EnableJoining();
        }

        public void OnResetAll()
        {
            PlayerManager.Instance.ClearRegisters();
            PlayerManager.Instance.ResetAllPlayers();
            EconomyManager.Instance.ClearRegisters();
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
                    AudioManager.Instance.PlayVictory();
                    StartCoroutine(BackToLobby());
                }
            }
        }

        private IEnumerator ReadyupTimer()
        {
            for (int i = 3; i > 0; i--)
            {
                _timerText.text = i.ToString();
                _timerAnimator.Play("TimerCount", 0, 0f);
                yield return new WaitForSeconds(1f);
            }
            _timerText.text = "";
            _infoText.text = string.Empty;
            _inputManager.DisableJoining();
            StartCoroutine(ReloadScene());
            PlayerManager.Instance.ResetAllPlayers();
            AudioManager.Instance.StartGame();
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
            SceneTransform = new GameObject("Container").transform;
            SceneManager.MoveGameObjectToScene(SceneTransform.gameObject, SceneManager.GetSceneByName("Lobby"));
            _inputManager.EnableJoining();
            AudioManager.Instance.StartLobby();
        }

        private IEnumerator ReloadScene()
        {
            yield return SceneManager.UnloadSceneAsync("Lobby");
            OnResetAll();
            yield return SceneManager.LoadSceneAsync(ResourceManager.Instance.GameInfo.GameLevel.Name, LoadSceneMode.Additive);
            SceneTransform = new GameObject("Container").transform;
            SceneManager.MoveGameObjectToScene(SceneTransform.gameObject, SceneManager.GetSceneByName(ResourceManager.Instance.GameInfo.GameLevel.Name));
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