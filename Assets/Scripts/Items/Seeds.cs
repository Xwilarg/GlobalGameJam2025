using GGJ.Player;
using GGJ.Prop;
using UnityEngine;

public class Seeds : MonoBehaviour, IInteractible, ITakeable
{
    public GameObject GameObject => gameObject;

    public bool CanBeSold => false;
    public bool CanWater => false;
    public bool CanCut => false;
    public bool CanPlant => true;
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