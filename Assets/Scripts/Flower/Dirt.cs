using GGJ.Manager;
using GGJ.Player;
using GGJ.Prop;
using UnityEngine;

public class Dirt : MonoBehaviour, IInteractible
{
    Flower flower;
    public Flower Flower { set => flower = value; }


    int flowerCount = 0;


    public bool Interact(PlayerController pc)
    {
        if (!flower/* && player carry seeds*/)
        {
            PlantFlower();
            return true;
        }

        return false;
    }

    void PlantFlower()
    {
        if (flower)
            return;

        flower = Instantiate(ResourceManager.Instance.FlowerPrefab, transform.position, transform.rotation).GetComponent<Flower>();
        flower.Dirt = this;

        flowerCount++;
    }
}
