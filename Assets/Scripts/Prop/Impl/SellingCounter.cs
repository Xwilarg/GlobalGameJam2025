using GGJ.Manager;
using GGJ.Player;
using TMPro;
using UnityEngine;

namespace GGJ.Prop.Impl
{
    public class SellingCounter : MonoBehaviour, IInteractible
    {
        [SerializeField]
        private TMP_Text _priceText;

        private void Start()
        {
            TimeManager.Instance.OnNewDay.AddListener(() =>
            {
                UpdateUI();
            });
            UpdateUI();
        }

        public bool CanInteract(PlayerController pc)
        {
            return pc.CarriedObject != null && pc.CarriedObject.CanBeSold;
        }

        private void UpdateUI()
        {
            _priceText.text = $"{GetCurrentPrice()}ƒ";
        }

        private int GetCurrentPrice()
        {
            var time01 = TimeManager.Instance.Day01;
            var info = ResourceManager.Instance.GameInfo;
            var value = (GameManager.Instance.GamePhase == GamePhase.PriceRaise ? info.RaisePriceCurve : info.CrashPriceCurve).Evaluate(time01);
            value *= (info.MinMaxPrice.Max - info.MinMaxPrice.Min);
            value += info.MinMaxPrice.Min;
            return Mathf.FloorToInt(value);
        }

        public void Interact(PlayerController pc)
        {

            pc.GainMoney(GetCurrentPrice());
            pc.DiscardCarry();
        }
    }
}