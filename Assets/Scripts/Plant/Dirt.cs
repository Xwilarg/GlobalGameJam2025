using GGJ.Manager;
using GGJ.Player;
using UnityEngine;

public class Dirt : MonoBehaviour
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

    public void Interact(PlayerController pc)
    {
        if (!plant && !pc.IsReady && pc.CarriedObject != null && pc.CarriedObject.CanPlant)
        {
            Color playerColor = pc.Color;
            PlantPlant(playerColor, pc.Id);
            pc.Ready();
        }
    }
}
