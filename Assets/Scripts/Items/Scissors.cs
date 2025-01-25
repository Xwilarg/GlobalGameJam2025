using GGJ.Player;
using GGJ.Prop;
using UnityEngine;

public class Scissors : MonoBehaviour, IInteractible, ITakeable
{
    public GameObject GameObject => gameObject;

    public bool CanBeSold => false;
    public bool CanWater => false;
    public bool CanCut => true;
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