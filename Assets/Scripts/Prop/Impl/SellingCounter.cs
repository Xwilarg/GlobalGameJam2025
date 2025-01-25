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
            UpdateUI();
        }

        public bool CanInteract(PlayerController pc)
        {
            return (GameManager.Instance.GamePhase == GamePhase.PriceRaise || GameManager.Instance.GamePhase == GamePhase.PriceCrash) && pc.CarriedObject != null && pc.CarriedObject.CanBeSold;
        }

        public string AddSign(int nb)
        {
            if (nb >= 0f) return $"+{nb}";
            else return $"-{Mathf.Abs(nb)}";
        }
        public void UpdateUI()
        {
            if (GameManager.Instance.GamePhase == GamePhase.GameEnded) _priceText.text = "Closed";
            else _priceText.text = $"{Mathf.RoundToInt(EconomyManager.Instance.CurrentPrice * Variation)}f ({AddSign(Mathf.RoundToInt(Variation))})";
        }

        public void UpdateVariation(float average)
        {
            Variation = (1f + Variation) / 2f;
            Variation += average - AmountSold;
        }

        public void Interact(PlayerController pc)
        {
            int money = Mathf.RoundToInt(EconomyManager.Instance.CurrentPrice * Variation);

            if (pc.CarriedObject != null &&
                pc.CarriedObject is CutedPlant &&
                ((CutedPlant)pc.CarriedObject).PlayerId != pc.Id)
                money = (int)(money * ResourceManager.Instance.GameInfo.OtherPlayerPlantPriceCoef);

            AmountSold++;
            pc.GainMoney(money);
            pc.DiscardCarry();
        }
    }
}