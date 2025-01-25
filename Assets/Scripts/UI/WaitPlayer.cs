using UnityEngine;
using UnityEngine.InputSystem;

public class WaitPlayer : MonoBehaviour
{
    [SerializeField, Range(1, 4)] int playerIdx = 1;
    [SerializeField] GameObject waitTextGO;
    [SerializeField] GameObject readyTextGO;

    bool isReady = false;
    public bool IsReady { get => isReady; }



    void SetIsReady(bool isReady)
    {
        this.isReady = isReady;

        waitTextGO.SetActive(!isReady);
        readyTextGO.SetActive(isReady);

        WaitPlayersScreen.Instance.CheckAllPlayersAreReady();
    }

    void Start()
    {
        SetIsReady(false);
    }


    public void OnAction(InputAction.CallbackContext value)
    {
        int controllerPlayerIdx = playerIdx; // TODO : check que c'est le bon controller

        if (playerIdx == controllerPlayerIdx && 
            value.phase == InputActionPhase.Started &&
            !isReady)
            SetIsReady(true);
    }
}
