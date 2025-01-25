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

    private void Start()
    {
        GameManager.Instance.Register(this);
    }

    void PlantPlant(Color flowerColor)
    {
        if (plant)
            return;

        plant = Instantiate(_plantPrefab, transform.position, transform.rotation).GetComponent<Plant>();
        plant.Dirt = this;
        plant.transform.parent = plant.Dirt.transform;

        plant.SetFlowerColor(flowerColor);

        plantCount++;
    }

    void IInteractible.Interact(PlayerController pc)
    {
        Color playerColor = pc.Color;
        PlantPlant(playerColor);
        pc.Ready();
    }

    public bool CanInteract(PlayerController pc)
    {
        return !plant && !pc.IsReady && pc.CarriedObject != null && pc.CarriedObject.CanPlant;
    }
}
