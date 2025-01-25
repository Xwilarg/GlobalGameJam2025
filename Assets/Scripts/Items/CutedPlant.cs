using GGJ.Player;
using GGJ.Prop;
using UnityEngine;

public class CutedPlant : MonoBehaviour, ITakeable
{
    public GameObject GameObject => gameObject;

    public bool CanBeSold => true;
    public bool CanWater => false;
    public bool CanCut => false;
    public bool CanPlant => false;
    public bool IsSwitchAllowed => false;
    public bool IsReserved { get; set; }

    [SerializeField] SpriteRenderer flowerSpriteRenderer;

    private int playerId;
    public int PlayerId { get => playerId; set => playerId = value; }


    public void SetFlowerColor(Color color)
    {
        flowerSpriteRenderer.color = color;
    }
}