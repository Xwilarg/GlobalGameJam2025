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
    public bool IsSwitchAllowed => false;

    [SerializeField] SpriteRenderer flowerSpriteRenderer;

    private int playerId;
    public int PlayerId { get => playerId; }


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
        pc.CutedPlantFlowerSprite.color = flowerSpriteRenderer.color;
        playerId = pc.Id;
    }
}