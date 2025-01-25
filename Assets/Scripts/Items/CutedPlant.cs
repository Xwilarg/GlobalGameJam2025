using GGJ.Player;
using GGJ.Prop;
using UnityEngine;

public class CutedPlant : MonoBehaviour, IInteractible, ITakeable
{
    public GameObject GameObject => gameObject;

    public bool CanBeSold => true;
    public bool CanWater => false;
    public bool CanCut => false;
    public bool CanPlant => false;

    [SerializeField] SpriteRenderer flowerSpriteRenderer;


    public void SetFlowerColor(Color color)
    {
        flowerSpriteRenderer.color = color;
    }

    public bool CanInteract(PlayerController pc)
    {
        return pc.CarriedObject == null;
    }

    public void Interact(PlayerController pc)
    {
        pc.Carry(this);
    }
}