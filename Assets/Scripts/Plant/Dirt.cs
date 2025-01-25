using GGJ.Manager;
using GGJ.Player;
using GGJ.Prop;
using UnityEngine;

public class Dirt : MonoBehaviour, IInteractible
{
    [SerializeField]
    private GameObject _plantPrefab;

    Plant plant;
    public Plant Plant { set => plant = value; }

    int plantCount = 0;


    void PlantPlant(Color flowerColor)
    {
        if (plant)
            return;

        plant = Instantiate(_plantPrefab, transform.position, transform.rotation).GetComponent<Plant>();
        plant.Dirt = this;

        plant.SetFlowerColor(flowerColor);

        plantCount++;
    }

    void IInteractible.Interact(PlayerController pc)
    {
        Color playerColor = Color.blue; // TODO : get color from player;
        PlantPlant(playerColor);
    }

    public bool CanInteract(PlayerController pc)
    {
        return !plant && pc.CarriedObject.GameObject && pc.CarriedObject.CanPlant;
    }
}
