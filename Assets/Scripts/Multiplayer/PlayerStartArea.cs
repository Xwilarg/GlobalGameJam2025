using GGJ.Manager;
using UnityEngine;

namespace GGJ.Multiplayer
{
    public class PlayerStartArea : MonoBehaviour
    {
        private void Start()
        {
            PlayerManager.Instance.Register(transform);
        }
    }
}