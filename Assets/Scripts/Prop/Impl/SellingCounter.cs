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

        public int AmountSold { set; get; }
        public float Variation { set; get; } = 1f;

        private void Start()
        {
            EconomyManager.Instance.Register(this);
            _priceText.text = "Closed";
        }

        public bool CanInteract(PlayerController pc)
        {
            return (GameManager.Instance.GamePhase == GamePhase.PriceRaise || GameManager.Instance.GamePhase == GamePhase.PriceCrash) && pc.CarriedObject != null && pc.CarriedObject.CanBeSold;
        }

        public string AddSign(float nb)
        {
            if (nb >= 0f) return $"+{nb}";
            else return $"-{Mathf.Abs(nb)}";
        }
        public void UpdateUI()
        {
            if (GameManager.Instance.GamePhase == GamePhase.GameEnded) _priceText.text = "Closed";
            else _priceText.text = $"{EconomyManager.Instance.CurrentPrice * Variation}ƒ (+{AddSign(Variation):0.00})";
        }

        public void UpdateVariation(float average)
        {
            Variation = (1f + Variation) / 2f;
            Variation += AmountSold - average;
        }

        public void Interact(PlayerController pc)
        {
            AmountSold++;
            pc.GainMoney(EconomyManager.Instance.CurrentPrice);
            pc.DiscardCarry();
        }
    }
}