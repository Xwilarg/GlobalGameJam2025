using GGJ.Manager;
using GGJ.Player;
using GGJ.Prop;
using UnityEngine;

public class Dirt : MonoBehaviour, IInteractible
{
    [SerializeField]
    private GameObject _plantPrefab;

    [SerializeField]
    private Sprite[] _sprites;

    Plant plant;
    public Plant Plant { set => plant = value; }

    int plantCount = 0;

    private void Awake()
    {
        GetComponent<SpriteRenderer>().sprite = _sprites[Random.Range(0, _sprites.Length)];
    }

    private void Start()
    {
        GameManager.Instance.Register(this);
    }

    void PlantPlant(Color flowerColor, int id)
    {
        if (plant)
            return;

        plant = Instantiate(_plantPrefab, transform.position, transform.rotation).GetComponent<Plant>();
        plant.Dirt = this;
        plant.transform.parent = plant.Dirt.transform;
        plant.PlayerId = id;

        plant.SetFlowerColor(flowerColor);

        plantCount++;
        AudioManager.Instance.PlayPlant();
    }

    void IInteractible.Interact(PlayerController pc)
    {
        Color playerColor = pc.Color;
        PlantPlant(playerColor, pc.Id);
        pc.Ready();
    }

    public bool CanInteract(PlayerController pc)
    {
        return !plant && !pc.IsReady && pc.CarriedObject != null && pc.CarriedObject.CanPlant;
    }
}
