using GGJ.Manager;
using GGJ.Player;
using UnityEngine;

namespace GGJ.Prop.Impl
{
    public class SellingCounter : MonoBehaviour, IInteractible
    {
        public bool Interact(PlayerController pc)
        {
            if (pc.CarriedObject != null && pc.CarriedObject.CanBeSold)
            {
                var time01 = 0f; // TODO
                var info = ResourceManager.Instance.GameInfo;
                var value = info.PriceCurve.Evaluate(0f);
                value *= (info.MinMaxPrice.Max - info.MinMaxPrice.Min);
                value += info.MinMaxPrice.Min;

                pc.GainMoney(Mathf.FloorToInt(value));
            }
            return true;
        }
    }
}