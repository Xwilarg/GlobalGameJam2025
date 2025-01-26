using UnityEngine;
using UnityEngine.Events;

namespace GGJ.Manager
{
    public class TimeManager : MonoBehaviour
    {
        public static TimeManager Instance { private set; get; }

        /// <summary>
        /// Value between 0 and 1 representing how much time elapsed in the current phase
        /// </summary>
        public float Day01 => _day / (float)(ResourceManager.Instance.GameInfo.RaisePhaseDuration - 1);
        public float TimeWithinDay01 => _timer / ResourceManager.Instance.GameInfo.DayDuration;

        private int _day;

        private bool _isTimerStarted;
        private float _timer;

        public UnityEvent OnNewDay { get; } = new();

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            GameManager.Instance.OnNextPhase.AddListener((phase) =>
            {
                if (phase == GamePhase.PriceRaise)
                {
                    _isTimerStarted = true;
                }
            });
        }

        private void Update()
        {
            if (!_isTimerStarted) return;

            var info = ResourceManager.Instance.GameInfo;
            _timer += Time.deltaTime;
            if (_timer >= info.DayDuration)
            {
                _timer = 0f;
                _day++;
                if (GameManager.Instance.GamePhase == GamePhase.PriceRaise)
                {
                    if (_day == info.RaisePhaseDuration)
                    {
                        _day = 0;
                        GameManager.Instance.SetPhase(GamePhase.PriceCrash);
                    }
                }
                else
                {
                    if (_day == info.CrashPhaseDuration)
                    {
                        _day = 0;
                        GameManager.Instance.SetPhase(GamePhase.GameEnded);
                    }
                }
                OnNewDay.Invoke();
            }
        }
    }
}