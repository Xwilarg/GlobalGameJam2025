using GGJ.Manager;
using GGJ.Prop;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GGJ.Player
{
    public class PlayerController : MonoBehaviour
    {
        private Vector2 _mov;
        private Vector2 _direction = Vector2.up;
        private Rigidbody2D _rb;

        private int _money;

        public ITakeable CarriedObject { private set; get; }

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            PlayerManager.Instance.Register(this);
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
                var coll = Physics2D.OverlapCircle(transform.position + (Vector3)_direction * ResourceManager.Instance.PlayerInfo.InteractionDistance, ResourceManager.Instance.PlayerInfo.InteractionSize, LayerMask.GetMask("Prop"));
                if (coll != null && coll.TryGetComponent<IInteractible>(out var interact))
                {
                    interact.Interact(this);
                }
            }
        }

        /// <summary>
        /// Attempt to carry an object
        /// </summary>
        /// <returns>Could the object be carried</returns>
        public bool Carry(ITakeable takeable)
        {
            if (CarriedObject != null) return false;

            CarriedObject = takeable;
            CarriedObject.GameObject.SetActive(false);
            return true;
        }

        public override string ToString()
        {
            return $"Player (Money: {_money})";
        }
    }
}
