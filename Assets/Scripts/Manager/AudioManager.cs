using UnityEngine;

namespace GGJ.Manager
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField]
        private float _maxVolume;

        [SerializeField]
        private AudioSource _bgmHappy, _bgmSad, _bgmNeutral;

        private void Awake()
        {
            _bgmHappy.volume = _maxVolume / 2f;
            _bgmSad.volume = _maxVolume / 2f;
        }
    }
}