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

        private void Awake()
        {
            Instance = this;
        }

        public void Register(PlayerController pc)
        {
            pc.transform.position = _startAreas[_players.Count % _startAreas.Count].position;
            _players.Add(pc);
        }

        public void Register(Transform start)
        {
            _startAreas.Add(start);
        }

        // TODO: Unregister

        public IEnumerable<string> PlayerStrings => _players.Select(x => x.ToString());
    }
}