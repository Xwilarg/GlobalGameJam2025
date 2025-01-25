using System.Collections;
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
        private float _day01;
        public float Day01
        {
            private set
            {
                _day01 = value;
                OnNewDay.Invoke();
            }
            get
            {
                return _day01;
            }
        }

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
                    StartCoroutine(WaitDays());
                }
            });

            // TODO: Remove following line once player management is done
            GameManager.Instance.SetPhase(GamePhase.PriceRaise);
        }

        private IEnumerator WaitDays()
        {
            var info = ResourceManager.Instance.GameInfo;
            for (int i = 0; i < info.RaisePhaseDuration; i++)
            {
                Day01 = i / (float)info.RaisePhaseDuration;
                yield return new WaitForSeconds(info.DayDuration);
            }
            GameManager.Instance.SetPhase(GamePhase.PriceCrash);
            for (int i = 0; i < info.CrashPhaseDuration; i++)
            {
                Day01 = i / (float)info.CrashPhaseDuration;
                yield return new WaitForSeconds(info.DayDuration);
            }
        }
    }
}