using UnityEngine;

namespace GGJ.Manager
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { private set; get; }

        [SerializeField]
        private float _maxVolume;

        [SerializeField]
        private AudioSource _bgmHappy, _bgmSad, _bgmNeutral, _bgmLobby;

        [SerializeField]
        private AudioSource _water, _cut, _plant, _punch, _sell, _newPlayer;

        private void Awake()
        {
            Instance = this;

            _bgmLobby.volume = _maxVolume;
        }

        public void StartLobby()
        {
            _bgmHappy.Stop();
            _bgmSad.Stop();
            _bgmNeutral.Stop();
            _bgmLobby.Play();
        }

        public void StartGame()
        {
            _bgmLobby.Stop();
            _bgmHappy.volume = 0f;
            _bgmSad.volume = 0f;
            _bgmNeutral.volume = _maxVolume;
            _bgmHappy.Play();
            _bgmSad.Play();
            _bgmNeutral.Play();
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
                _bgmHappy.volume = ResourceManager.Instance.GameInfo.AudioCurveIncrease.Evaluate(val01) * _maxVolume;
                _bgmNeutral.volume = ResourceManager.Instance.GameInfo.AudioCurveDecrease.Evaluate(val01) * _maxVolume;
                _bgmSad.volume = 0f;
            }
            else
            {
                var val01 = ((TimeManager.Instance.Day01 * 10f) + TimeManager.Instance.TimeWithinDay01) / 10f;
                _bgmSad.volume = ResourceManager.Instance.GameInfo.AudioCurveIncrease.Evaluate(val01) * _maxVolume;
                _bgmHappy.volume = ResourceManager.Instance.GameInfo.AudioCurveDecrease.Evaluate(val01) * _maxVolume;
                _bgmNeutral.volume = 0f;
            }
        }

        public void PlayWater() => _water.PlayOneShot(_water.clip);
        public void PlayCut() => _cut.PlayOneShot(_cut.clip);
        public void PlayPlant() => _plant.PlayOneShot(_plant.clip);
        public void PlayPunch() => _punch.PlayOneShot(_punch.clip);
        public void PlaySell() => _sell.PlayOneShot(_sell.clip);
        public void PlayNewPlayer() => _newPlayer.PlayOneShot(_newPlayer.clip);
    }
}