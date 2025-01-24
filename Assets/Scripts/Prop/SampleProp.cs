using GGJ.Player;
using UnityEngine;

namespace GGJ.Prop
{
    public class SampleProp : MonoBehaviour, IInteractible, ITakeable
    {
        public GameObject GameObject => gameObject;

        public bool Interact(PlayerController pc)
        {
            pc.Carry(this);
            return true;
        }
    }
}