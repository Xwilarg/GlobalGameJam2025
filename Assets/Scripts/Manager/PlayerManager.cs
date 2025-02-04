using GGJ.Player;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GGJ.Manager
{
    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager Instance { private set; get; }

        private readonly List<PlayerController> _players = new();
        private readonly List<Transform> _startAreas = new();

        [SerializeField]
        private Color[] _colors;

        [SerializeField]
        private PlayerScore[] _pScores;

        public List<PlayerController> Players { get => _players; }

        private void Awake()
        {
            Instance = this;
        }

        public void ResetSpawns()
        {
            for (int i = 0; i < _players.Count; i++)
            {
                var pc = _players[i];
                var spawn = _startAreas[i].position;
                pc.transform.position = spawn;
                pc.SpawnPoint = spawn;
            }
        }

        public void Register(PlayerController pc)
        {
            var spawn = _startAreas[_players.Count % _startAreas.Count].position;
            pc.transform.position = spawn;
            pc.SpawnPoint = spawn;
            pc.Color = _colors[_players.Count % _colors.Length];
            pc.PlayerScore = _pScores[_players.Count % _pScores.Length];
            pc.PlayerScore.gameObject.SetActive(true);
            _players.Add(pc);
            pc.Id = _players.Count;

            AudioManager.Instance.PlayNewPlayer();

            if (_players.Count == 2)
            {
                GameManager.Instance.ShowReadyPendingText();
            }
        }

        public void ClearRegisters()
        {
            _startAreas.Clear();
        }

        public void Register(Transform start)
        {
            _startAreas.Add(start);
        }

        public void CheckAllReady()
        {
#if !UNITY_EDITOR
            if (_players.Count < 2) return;
#endif

            if (_players.All(x => x.IsReady))
            {
                GameManager.Instance.SetPhase(GamePhase.PriceRaise);
            }
        }

        public void ResetAllPlayers()
        {
            foreach (var p in _players)
            {
                p.ResetAll();
                p.UnreadyForGameStart();
            }
            foreach (var sc in _pScores)
            {
                sc.SetScore(0);
            }
        }

        // TODO: Unregister

        public IEnumerable<string> PlayerStrings => _players.Select(x => x.ToString());
    }
}