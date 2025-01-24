using GGJ.Manager;
using GGJ.Prop;
using GGJ.SO;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GGJ.Player
{
    public class PlayerController : MonoBehaviour
    {
        private Vector2 _mov;
        private Vector2 _direction = Vector2.up;
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

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            if (ResourceManager.Instance != null)
            {
                Gizmos.DrawWireSphere(transform.position + (Vector3)_direction * ResourceManager.Instance.PlayerInfo.InteractionDistance, ResourceManager.Instance.PlayerInfo.InteractionSize);
            }
        }

        public void OnMovement(InputAction.CallbackContext value)
        {
            _mov = value.ReadValue<Vector2>();
            if (_mov.magnitude > 0f)
            {
                _direction = _mov;
            }
        }

        public void OnAction(InputAction.CallbackContext value)
        {
            if (value.phase == InputActionPhase.Started)
            {

            }
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
