using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

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
            _bgmHappy.volume = 0f;
            _bgmSad.volume = 0f;
            _bgmNeutral.volume = _maxVolume;
        }

        private void Update()
        {
            if (GameManager.Instance.GamePhase == GamePhase.LobbyPreparation)
            {
                _bgmHappy.volume = 0f;
                _bgmSad.volume = 0f;
                _bgmNeutral.volume = _maxVolume;
            }
            else if (GameManager.Instance.GamePhase == GamePhase.PriceRaise)
            {
                var val01 = ((TimeManager.Instance.Day01 * 10f) + TimeManager.Instance.TimeWithinDay01) / 10f;
                _bgmHappy.volume = val01 * _maxVolume;
                _bgmNeutral.volume = _maxVolume - (val01 * _maxVolume);
                _bgmSad.volume = 0f;
            }
            else
            {
                var val01 = ((TimeManager.Instance.Day01 * 10f) + TimeManager.Instance.TimeWithinDay01) / 10f;
                _bgmSad.volume = val01 * _maxVolume;
                _bgmHappy.volume = _maxVolume - (val01 * _maxVolume);
                _bgmNeutral.volume = 0f;
            }
        }
    }
}