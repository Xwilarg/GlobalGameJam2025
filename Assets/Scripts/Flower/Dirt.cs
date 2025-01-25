using GGJ.Manager;
using GGJ.Player;
using GGJ.Prop;
using UnityEngine;

public class Dirt : MonoBehaviour, IInteractible
{
    Flower flower;
    public Flower Flower { set => flower = value; }


    int flowerCount = 0;


    void PlantFlower()
    {
        if (flower)
            return;

        flower = Instantiate(ResourceManager.Instance.FlowerPrefab, transform.position, transform.rotation).GetComponent<Flower>();
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
