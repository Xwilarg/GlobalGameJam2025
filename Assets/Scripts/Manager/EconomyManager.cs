using UnityEngine;

namespace GGJ.Manager
{
    public class EconomyManager : MonoBehaviour
    {
        public static EconomyManager Instance { private set; get; }

        private void Awake()
        {
            Instance = this;
        }

        public int CurrentPrice
        {
            get
            {
                var time01 = TimeManager.Instance.Day01;
                var info = ResourceManager.Instance.GameInfo;
                var value = (GameManager.Instance.GamePhase == GamePhase.PriceRaise ? info.RaisePriceCurve : info.CrashPriceCurve).Evaluate(time01);
                value *= (info.MinMaxPrice.Max - info.MinMaxPrice.Min);
                value += info.MinMaxPrice.Min;
                return Mathf.FloorToInt(value);
            }
        }
    }
}