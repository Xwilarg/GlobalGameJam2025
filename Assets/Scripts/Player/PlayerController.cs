using GGJ.Manager;
using GGJ.Prop;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GGJ.Player
{
    public class PlayerController : MonoBehaviour
    {
        private Vector2 _mov;
        private Rigidbody2D _rb;

        private ITakeable _carriedObject;

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

        /// <summary>
        /// Attempt to carry an object
        /// </summary>
        /// <returns>Could the object be carried</returns>
        public bool Carry(ITakeable takeable)
        {
            if (_carriedObject != null) return false;

            _carriedObject = takeable;
            _carriedObject.GameObject.SetActive(false);
            return true;
        }
    }
}
