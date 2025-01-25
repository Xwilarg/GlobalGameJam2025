using GGJ.SO;
using UnityEngine;

namespace GGJ.Manager
{
    public class ResourceManager : MonoBehaviour
    {
        public static ResourceManager Instance { private set; get; }

        [SerializeField]
        private GameInfo _gameInfo;
        public GameInfo GameInfo => _gameInfo;

        [SerializeField]
        private PlantInfo _plantInfo;
        public PlantInfo PlantInfo => _plantInfo;

        private void Awake()
        {
            Instance = this;
        }
    }
}