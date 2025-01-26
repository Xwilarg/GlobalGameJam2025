using UnityEngine;

namespace GGJ.Manager
{
    public class RuntimeManager : MonoBehaviour
    {
        private void Start()
        {
            PlayerManager.Instance.ResetSpawns();
        }
    }
}