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

            }
            return true;
        }
    }
}