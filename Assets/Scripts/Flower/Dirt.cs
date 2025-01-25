using GGJ.Manager;
using GGJ.Player;
using GGJ.Prop;
using UnityEngine;

public class Dirt : MonoBehaviour, IInteractible
{
    [SerializeField]
    private GameObject _flowerPrefab;

    Flower flower;
    public Flower Flower { set => flower = value; }


    int flowerCount = 0;


    void PlantFlower()
    {
        if (flower)
            return;

        flower = Instantiate(_flowerPrefab, transform.position, transform.rotation).GetComponent<Flower>();
        flower.Dirt = this;

        flowerCount++;
    }

    void IInteractible.Interact(PlayerController pc)
    {
        PlantFlower();
    }

    public bool CanInteract(PlayerController pc)
    {
        return !flower && pc.CarriedObject.GameObject.name == "Seeds";
    }
}
