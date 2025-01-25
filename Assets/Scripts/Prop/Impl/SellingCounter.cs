using GGJ.Manager;
using GGJ.Player;
using UnityEngine;

namespace GGJ.Prop.Impl
{
    public class SellingCounter : MonoBehaviour, IInteractible
    {
        public bool CanInteract(PlayerController pc)
        {
            return pc.CarriedObject != null && pc.CarriedObject.CanBeSold;
        }

        public void Interact(PlayerController pc)
        {
            var time01 = TimeManager.Instance.Day01;
            var info = ResourceManager.Instance.GameInfo;
            var value = (GameManager.Instance.GamePhase == GamePhase.PriceRaise ? info.RaisePriceCurve : info.CrashPriceCurve).Evaluate(time01);
            value *= (info.MinMaxPrice.Max - info.MinMaxPrice.Min);
            value += info.MinMaxPrice.Min;

            pc.GainMoney(Mathf.FloorToInt(value));
            pc.DiscardCarry();
        }
    }
}