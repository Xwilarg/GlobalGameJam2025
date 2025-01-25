using GGJ.Player;
using GGJ.Prop;
using UnityEngine;

public class WateringCan: MonoBehaviour, IInteractible, ITakeable
{
    public GameObject GameObject => gameObject;

    public bool CanBeSold => false;
    public bool CanWater => true;
    public bool CanCut => false;
    public bool CanPlant => false;

    public bool CanInteract(PlayerController pc)
    {
        return pc.CarriedObject == null;
    }

    public void Interact(PlayerController pc)
    {
        pc.Carry(this);
    }
}