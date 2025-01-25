using GGJ.Prop.Impl;
using System.Collections.Generic;
using System.Linq;
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

        private void Start()
        {
            TimeManager.Instance.OnNewDay.AddListener(() =>
            {
                foreach (var c in _counters)
                {
                    c.UpdateUI();
                }
                var average = _counters.Sum(x => x.AmountSold) / (float)_counters.Count;
                foreach (var c in _counters)
                {
                    c.UpdateVariation(average);
                }
            });
        }

        private readonly List<SellingCounter> _counters = new();

        public void Register(SellingCounter counter)
        {
            _counters.Add(counter);
        }

        public void ClearRegisters()
        {
            _counters.Clear();
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