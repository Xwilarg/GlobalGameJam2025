using GGJ.SO;
using UnityEngine;

namespace GGJ.Manager
{
    public class ResourceManager : MonoBehaviour
    {
        public static ResourceManager Instance { private set; get; }

        [SerializeField]
        private GameInfo _playerInfo;
        public GameInfo PlayerInfo => _playerInfo;

        private void Awake()
        {
            Instance = this;
        }
    }
}