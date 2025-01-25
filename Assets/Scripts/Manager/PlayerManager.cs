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

        private void Awake()
        {
            Instance = this;
        }

        public void Register(PlayerController pc)
        {
            var spawn = _startAreas[_players.Count % _startAreas.Count].position;
            pc.transform.position = spawn;
            pc.SpawnPoint = spawn;
            pc.Color = _colors[_players.Count % _startAreas.Count];
            _players.Add(pc);
            pc.Id = _players.Count;

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
        }

        // TODO: Unregister

        public IEnumerable<string> PlayerStrings => _players.Select(x => x.ToString());
    }
}