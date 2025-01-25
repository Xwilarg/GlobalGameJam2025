using GGJ.Player;
using UnityEngine;

namespace GGJ.Prop.Impl
{
    public class SampleProp : MonoBehaviour, IInteractible, ITakeable
    {
        public GameObject GameObject => gameObject;

        public bool CanBeSold => true;
        public bool CanWater => false;
        public bool CanCut => false;
        public bool CanPlant => false;
        public bool IsSwitchAllowed => true;

        public bool CanInteract(PlayerController pc)
        {
            return pc.CarriedObject == null;
        }

        public void Interact(PlayerController pc)
        {
            pc.Carry(this);
        }
    }
}