using UnityEngine;

public class WaitPlayersScreen : MonoBehaviour
{
    public static WaitPlayersScreen Instance { private set; get; }

    [SerializeField] WaitPlayer waitPlayer1, waitPlayer2, waitPlayer3, waitPlayer4;

    void Awake()
    {
        Instance = this;
    }

    public void CheckAllPlayersAreReady()
    {
        if (waitPlayer1.IsReady &&
            waitPlayer2.IsReady &&
            waitPlayer3.IsReady &&
            waitPlayer4.IsReady)
            AllPlayersAreReady();
    }

    void AllPlayersAreReady()
    {
        StartGame();
    }

    void StartGame()
    {
        gameObject.SetActive(false);
    }
}
