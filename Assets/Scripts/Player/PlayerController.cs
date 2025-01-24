using GGJ.Manager;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GGJ.PlayerController
{
    public class PlayerController : MonoBehaviour
    {
        private Vector2 _mov;
        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            _rb.linearVelocity = _mov * ResourceManager.Instance.PlayerInfo.Speed;
        }

        public void OnMovement(InputAction.CallbackContext value)
        {
            _mov = value.ReadValue<Vector2>();
        }
    }
}
