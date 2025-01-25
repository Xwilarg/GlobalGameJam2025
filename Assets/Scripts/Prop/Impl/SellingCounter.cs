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
            _priceText.text = $"{EconomyManager.Instance.CurrentPrice}ƒ";
        }

        public void Interact(PlayerController pc)
        {

            pc.GainMoney(EconomyManager.Instance.CurrentPrice);
            pc.DiscardCarry();
        }
    }
}